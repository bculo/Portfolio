using Crypto.Application.Common.Options;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;

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
        private readonly SagaTimeoutOptions _options;
        private readonly ILogger<AddCryptoItemStateMachine> _logger;
        

        public MassTransit.State Delayed { get; set; } = default!;
        public MassTransit.State ProcessStarted { get; set; } = default!;

        public Event<AddItemWithDelay> AddCryptoItemWithDelay { get; set; } = default!;
        public Event<UndoAddItemWithDelay> UndoAddCryptoItemWithDelay { get; set; } = default!;
        public Event<NewItemAdded> NewCryptoAdded { get; set; } = default!;
        public Event<Fault<AddItem>> NewCryptoAddedError { get; set; } = default!;
        public Schedule<AddCryptoItemState, AddItemTimeoutExpired> NewCryptoItemTimeout { get; set; } = default!;

        public AddCryptoItemStateMachine(IOptions<SagaTimeoutOptions> options, 
            ILogger<AddCryptoItemStateMachine> logger, 
            IServiceProvider provider)
        {
            _logger = logger;
            _options = options.Value;

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
                    s.Delay = TimeSpan.FromSeconds(_options.TimeoutCryptoAddInSeconds);
                    s.Received = r =>  r.CorrelateById(context => context.Message.TemporaryId);
                });

            Initially(
                When(AddCryptoItemWithDelay)
                    .Then(x =>
                    {
                        x.Saga.Symbol = x.Message.Symbol;
                        _logger.LogInformation("Event {Event} invoked for symbol {Symbol}, {CorrelationId}",
                            nameof(AddCryptoItemWithDelay),
                            x.Message.Symbol,
                            x.Message.TemporaryId);
                    })
                    .Schedule(NewCryptoItemTimeout, context => 
                        context.Init<AddItemTimeoutExpired>(new
                        {
                            context.Message.TemporaryId,
                            context.Message.Symbol
                        }))
                    .TransitionTo(Delayed));

            During(Delayed,
                When(NewCryptoItemTimeout!.Received)
                    .Then(x =>
                    {
                        _logger.LogInformation("Timeout expired for {Symbol}, {CorrelationId}",
                            x.Message.Symbol,
                            x.Message.TemporaryId);
                    })
                    .Publish(x => new AddItem { Symbol = x.Saga.Symbol, TemporaryId = x.Saga.CorrelationId })
                    .TransitionTo(ProcessStarted),
                When(UndoAddCryptoItemWithDelay)
                    .Unschedule(NewCryptoItemTimeout)
                    .Finalize());

            During(ProcessStarted,
                When(NewCryptoAdded)
                    .Finalize(),
                When(NewCryptoAddedError)
                    .Publish(x => new AddItemFailed { Symbol = x.Saga.Symbol })
                    .Finalize(),            
                Ignore(UndoAddCryptoItemWithDelay));

            During(Final,
                Ignore(NewCryptoItemTimeout.AnyReceived),
                Ignore(UndoAddCryptoItemWithDelay));
        }
    }
}
