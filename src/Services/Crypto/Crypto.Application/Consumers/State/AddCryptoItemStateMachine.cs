using Crypto.Application.Options;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Crypto.Application.Consumers.State
{
    public class AddCryptoItemState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string Symbol { get; set; }
        public Guid? AddCryptoItemTimeoutId { get; set; }
    }

    public class AddCryptoItemStateMachine : MassTransitStateMachine<AddCryptoItemState>
    {
        public MassTransit.State Delayed { get; private set; }
        public MassTransit.State DelayExpired { get; private set; }
        public MassTransit.State DelayCanceled { get; private set; }
        public MassTransit.State CreationProcessStarted { get; private set; }
        public MassTransit.State CreationProcessFailed { get; private set; }
        public MassTransit.State CreationProcessFinished { get; private set; }

        public Event<AddCryptoItemWithDelay> AddCryptoItemWithDelay { get; private set; }
        public Event<UndoAddCryptoItemWithDelay> UndoAddCryptoItemWithDelay { get; private set; }
        public Event<NewCryptoAdded> NewCryptoAdded { get; private set; }
        public Event<Fault<AddCryptoItem>> CreateCryptoItemError { get; private set; }
        public Schedule<AddCryptoItemState, AddCryptoItemWithDelayTimeout> AddCryptoItemTimeout { get; private set; }

        public AddCryptoItemStateMachine(IOptions<SagaTimeoutOptions> options)
        {
            InstanceState(x => x.CurrentState);

            Event(() => AddCryptoItemWithDelay, x => x.CorrelateById(context => context.Message.TemporaryId));
            Event(() => UndoAddCryptoItemWithDelay, x => x.CorrelateById(context => context.Message.TemporaryId));
            Event(() => NewCryptoAdded, x => x.CorrelateById(context => context.Message.TemporaryId));
            Event(() => CreateCryptoItemError, x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.TemporaryId));

            Schedule(() => AddCryptoItemTimeout, instance => instance.AddCryptoItemTimeoutId, s =>
            {
                s.Delay = TimeSpan.FromSeconds(options.Value.TimeoutCryptoAddInSeconds);
                s.Received = r => r.CorrelateById(context => context.Message.TemporaryId);
            });

            Initially(
                When(AddCryptoItemWithDelay)
                    .Then(x => x.Saga.Symbol = x.Message.Symbol)
                    .Schedule(AddCryptoItemTimeout, context =>
                        context.Init<AddCryptoItemWithDelayTimeout>(new 
                        {
                            TemporaryId = context.Saga.CorrelationId
                        }))
                    .TransitionTo(Delayed));

            During(Delayed,
                When(AddCryptoItemTimeout.Received)
                    .ThenAsync(async x =>
                    {
                        var endpoint = await x.GetSendEndpoint(new Uri($"queue:AddCryptoItem"));
                        await endpoint.Send(new AddCryptoItem { Symbol = x.Saga.Symbol, TemporaryId = x.Saga.CorrelationId });
                    })
                    .TransitionTo(CreationProcessStarted),
                When(UndoAddCryptoItemWithDelay)
                    .Unschedule(AddCryptoItemTimeout)
                    .TransitionTo(DelayCanceled)
                    .Finalize());

            During(Delayed,
                Ignore(AddCryptoItemWithDelay));

            During(CreationProcessStarted,
                When(NewCryptoAdded)
                    .TransitionTo(CreationProcessFinished)
                    .Finalize(),
                When(CreateCryptoItemError)
                    .TransitionTo(CreationProcessFailed)
                    .Finalize(),            
                Ignore(UndoAddCryptoItemWithDelay));

            SetCompletedWhenFinalized();
        }
    }
}
