using Events.Common.User;
using MassTransit;
using MediatR;
using User.Application.Common.Serializer;
using User.Application.Interfaces;

namespace User.Application.Features;

public class UploadVerificationImageDto : IRequest
{
    
}

public class UploadVerificationImageHandler : IRequestHandler<UploadVerificationImageDto>
{
    private readonly IPublishEndpoint _publish;
    private readonly ICurrentUserService _currentUser;
    
    public UploadVerificationImageHandler(IPublishEndpoint publish, ICurrentUserService currentUser)
    {
        _publish = publish;
        _currentUser = currentUser;
    }
    
    public async Task Handle(UploadVerificationImageDto request, CancellationToken cancellationToken)
    {
        var imageUploaded = new UserImageUploadedBody
        {
            UserId = _currentUser.GetUserId()
        };
        
        var message = new UserImageUploaded
        {
            Body = imageUploaded,
            RawMessage = imageUploaded,
            MessageId = Guid.NewGuid(),
            CorrelationId = Guid.NewGuid()
        };

        await _publish.Publish(message, x =>
        {
            x.Serializer = new SystemTextJsonCustomRawMessageSerializer(RawSerializerOptions.All);
            x.MessageId = message.MessageId;
            x.CorrelationId = message.CorrelationId;
        }, cancellationToken);
    }
}