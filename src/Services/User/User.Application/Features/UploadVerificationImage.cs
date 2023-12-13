using System.Net.Mime;
using Events.Common.User;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using User.Application.Common.Serializer;
using User.Application.Interfaces;

namespace User.Application.Features;

public class UploadVerificationImageFormData
{
    public byte[] Image { get; set; }
}

public class UploadVerificationImageDto : IRequest
{
    public byte[] Image { get; set; }
    public string ContentType { get; set; }
    public string Name { get; set; }
}

public class UploadVerificationImageDtoValidator : AbstractValidator<UploadVerificationImageDto>
{
    public UploadVerificationImageDtoValidator()
    {
        RuleFor(i => i.Image).NotEmpty();
        RuleFor(i => i.ContentType).NotEmpty();
        RuleFor(i => i.Name).NotEmpty();
    }
}

public class UploadVerificationImageHandler : IRequestHandler<UploadVerificationImageDto>
{
    private readonly IPublishEndpoint _publish;
    private readonly ICurrentUserService _currentUser;
    private readonly IBlobStorage _blobStorage;
    
    public UploadVerificationImageHandler(IPublishEndpoint publish, 
        ICurrentUserService currentUser,
        IBlobStorage blobStorage)
    {
        _publish = publish;
        _currentUser = currentUser;
        _blobStorage = blobStorage;
    }
    
    public async Task Handle(UploadVerificationImageDto request, CancellationToken cancellationToken)
    {
        var imageName = GetBlobName();
        await _blobStorage.UploadBlob(imageName, request.Image, request.ContentType);
        
        var message = CreateMessage();
        await _publish.Publish(message, x =>
        {
            x.Serializer = new SystemTextJsonCustomRawMessageSerializer(RawSerializerOptions.All);
            x.MessageId = message.MessageId;
            x.CorrelationId = message.CorrelationId;
        }, cancellationToken);
    }

    private UserImageUploaded CreateMessage()
    {
        var imageUploaded = new UserImageUploadedBody
        {
            UserId = _currentUser.GetUserId(),
            ImageName = GetBlobName()
        };

        return new UserImageUploaded
        {
            Body = imageUploaded,
            RawMessage = imageUploaded,
            MessageId = Guid.NewGuid(),
            CorrelationId = Guid.NewGuid()
        };
    }

    private string GetBlobName()
    {
        return $"{_currentUser.GetUserId()}-original";
    }
}