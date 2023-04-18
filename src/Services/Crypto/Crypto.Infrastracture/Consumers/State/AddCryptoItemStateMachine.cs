using Crypto.Application.Options;
using Crypto.Infrastracture.Persistence;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Crypto.Infrastracture.Consumers.State
{
    public class AddCryptoItemState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string Symbol { get; set; }
        public Guid? AddCryptoItemTimeoutId { get; set; }
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
        public MassTransit.State Delayed { get; private set; }
        public MassTransit.State DelayExpired { get; private set; }
        public MassTransit.State DelayCanceled { get; private set; }
        public MassTransit.State CreationProcessStarted { get; private set; }

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
                s.Received = r =>
                {
                    r.CorrelateById(context => context.Message.TemporaryId);
                    r.OnMissingInstance(m => m.Discard());
                };
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
                        var endpoint = await x.GetSendEndpoint(new Uri($"queue:crypto-add-crypto-item"));
                        await endpoint.Send(new AddCryptoItem { Symbol = x.Saga.Symbol, TemporaryId = x.Saga.CorrelationId });
                    })
                    .TransitionTo(CreationProcessStarted),
                When(UndoAddCryptoItemWithDelay)
                    .Unschedule(AddCryptoItemTimeout)
                    .Finalize());

            During(CreationProcessStarted,
                When(NewCryptoAdded)
                    .Finalize(),
                When(CreateCryptoItemError)
                    .Publish(x => new AddCryptoItemFailed { Symbol = x.Saga.Symbol })
                    .Finalize(),            
                Ignore(UndoAddCryptoItemWithDelay));

            During(Final,
                Ignore(AddCryptoItemTimeout.AnyReceived),
                Ignore(UndoAddCryptoItemWithDelay));
        }
    }

    public class AddCryptoItemStateMachineDefinition : SagaDefinition<AddCryptoItemState>
    {
        private readonly IServiceProvider _provider;

        public AddCryptoItemStateMachineDefinition(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<AddCryptoItemState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 500, 1000));
        }
    }
}
