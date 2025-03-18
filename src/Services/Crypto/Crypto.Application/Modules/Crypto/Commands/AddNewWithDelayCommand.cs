using Crypto.Application.Common.Extensions;
using Events.Common.Crypto;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands;

public record AddNewWithDelayCommand(string Symbol) : IRequest<Guid>;

public class AddNewWithDelayCommandValidator : AbstractValidator<AddNewWithDelayCommand>
{
    public AddNewWithDelayCommandValidator()
    { 
        RuleFor(i => i.Symbol).WithSymbolRule();
    }
}

public class AddNewWithDelayCommandHandler(IPublishEndpoint publish) : IRequestHandler<AddNewWithDelayCommand, Guid>
{
    public async Task<Guid> Handle(AddNewWithDelayCommand request, CancellationToken cancellationToken)
    {
        var temporaryId = Guid.NewGuid();

        await publish.Publish(new AddItemWithDelay
        {
            Symbol = request.Symbol,
            CorrelationId = temporaryId
        }, cancellationToken);

        return temporaryId;
    }
}