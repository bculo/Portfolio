using Events.Common.User;
using MassTransit;
using MediatR;
using User.Application.Common.Serializer;

namespace User.Application.Features;

public class UploadVerificationImageDto : IRequest
{
    
}

public class UploadVerificationImageHandler : IRequestHandler<UploadVerificationImageDto>
{
    private readonly IPublishEndpoint _publish;
    
    public UploadVerificationImageHandler(IPublishEndpoint publish)
    {
        _publish = publish;
    }
    
    public async Task Handle(UploadVerificationImageDto request, CancellationToken cancellationToken)
    {
        var imageUploaded = new UserImageUploadedBody { UserId = 2 };
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