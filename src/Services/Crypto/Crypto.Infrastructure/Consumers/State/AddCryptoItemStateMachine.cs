using Crypto.Application.Options;
using Crypto.Infrastructure.Persistence;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Crypto.Infrastructure.Consumers.State
{
    public class AddCryptoItemState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = default!;
        public string Symbol { get; set; } = default!;
        public Guid? AddCryptoItemTimeoutTokenId { get; set; }
    }

    public class AddCryptoItemStateMap : SagaClassMap<AddCryptoItemState>
    {
        protected override void Configure(EntityTypeBuilder<AddCryptoItemState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.Symbol).HasMaxLength(10);
        }
    }

    public class AddCryptoItemStateMachine : MassTransitStateMachine<AddCryptoItemState>
    {
        public MassTransit.State Delayed { get; set; } = default!;
        public MassTransit.State ProcessStarted { get; set; } = default!;

        public Event<AddCryptoItemWithDelay> AddCryptoItemWithDelay { get; set; } = default!;
        public Event<UndoAddCryptoItemWithDelay> UndoAddCryptoItemWithDelay { get; set; } = default!;
        public Event<NewCryptoAdded> NewCryptoAdded { get; set; } = default!;
        public Event<Fault<AddCryptoItem>> NewCryptoAddedError { get; set; } = default!;
        public Schedule<AddCryptoItemState, AddCryptoItemWithDelayTimeoutExpired> NewCryptoItemTimeout { get; set; } = default!;

        public AddCryptoItemStateMachine(IOptions<SagaTimeoutOptions> options)
        {
            InstanceState(x => x.CurrentState);
          
            Event(() => AddCryptoItemWithDelay, 
                x => x.CorrelateById(context => context.Message.TemporaryId));
            Event(() => UndoAddCryptoItemWithDelay, 
                x => x.CorrelateById(context => context.Message.TemporaryId));
            Event(() => NewCryptoAdded, 
                x => x.CorrelateById(context => context.Message.TemporaryId));
            Event(() => NewCryptoAddedError, 
                x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.TemporaryId));
            Schedule(() => NewCryptoItemTimeout, 
                instance => instance.AddCryptoItemTimeoutTokenId, 
                s => 
                {
                    s.Delay = TimeSpan.FromSeconds(options.Value.TimeoutCryptoAddInSeconds);
                    s.Received = r =>
                    {
                        r.CorrelateById(context => context.Message.TemporaryId);
                    };
                });

            Initially(
                When(AddCryptoItemWithDelay)
                    .Then(x => x.Saga.Symbol = x.Message.Symbol)
                    .Schedule(NewCryptoItemTimeout, context => 
                        context.Init<AddCryptoItemWithDelayTimeoutExpired>(new { context.Message.TemporaryId }))
                    .TransitionTo(Delayed));

            During(Delayed,
                When(NewCryptoItemTimeout!.Received)
                    .Publish(x => new AddCryptoItem { Symbol = x.Saga.Symbol, TemporaryId = x.Saga.CorrelationId })
                    .TransitionTo(ProcessStarted),
                When(UndoAddCryptoItemWithDelay)
                    .Unschedule(NewCryptoItemTimeout)
                    .Finalize());

            During(ProcessStarted,
                When(NewCryptoAdded)
                    .Finalize(),
                When(NewCryptoAddedError)
                    .Publish(x => new AddCryptoItemFailed { Symbol = x.Saga.Symbol })
                    .Finalize(),            
                Ignore(UndoAddCryptoItemWithDelay));

            During(Final,
                Ignore(NewCryptoItemTimeout.AnyReceived),
                Ignore(UndoAddCryptoItemWithDelay));
        }
    }
}
