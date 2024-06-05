using System.Collections.Frozen;
using Events.Common.User;
using Events.MassTransit.Common.Serializers;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using User.Application.Common.Options;
using User.Application.Interfaces;

namespace User.Application.Features;

public record UploadVerificationImageFormData
{
    public byte[] Image { get; set; } = default!;
}

public record UploadVerificationImageDto : IRequest
{
    public byte[] Image { get; init; } = default!;
    public string ContentType { get; init; } = default!;
    public string Name { get; init; } = default!;
}

public class UploadVerificationImageDtoValidator : AbstractValidator<UploadVerificationImageDto>
{
    private static readonly FrozenSet<string> AllowedContentType = new HashSet<string>
    {
        "image/jpeg",
        "image/jpg",
        "image/png"
    }.ToFrozenSet();

    public UploadVerificationImageDtoValidator()
    {
        RuleFor(i => i.Image)
            .NotEmpty();
        
        RuleFor(i => i.ContentType)
            .Must(IsAllowedType)
            .WithMessage("Content type must be 'jpeg' or 'png'")
            .NotEmpty();
        
        RuleFor(i => i.Name)
            .NotEmpty();
    }

    private bool IsAllowedType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return false;
        }

        return AllowedContentType.Contains(contentType);
    }
}

public class UploadVerificationImageHandler : IRequestHandler<UploadVerificationImageDto>
{
    private readonly IPublishEndpoint _publish;
    private readonly ICurrentUserService _currentUser;
    private readonly IBlobStorage _blobStorage;
    private readonly IImageService _imageService;
    private readonly BlobStorageOptions _blobStorageOptions;
    
    public UploadVerificationImageHandler(IPublishEndpoint publish, 
        ICurrentUserService currentUser,
        IBlobStorage blobStorage,
        IOptions<BlobStorageOptions> blobStorageOptions, 
        IImageService imageService)
    {
        _publish = publish;
        _currentUser = currentUser;
        _blobStorage = blobStorage;
        _imageService = imageService;
        _blobStorageOptions = blobStorageOptions.Value;
    }
    
    public async Task Handle(UploadVerificationImageDto request, CancellationToken cancellationToken)
    {
        await using var stream = await _imageService.ResizeImage(request.Image, 720, 480);
        
        var imageName = GetBlobName();
        var uri = await _blobStorage.UploadBlob(_blobStorageOptions.VerificationContainerName, imageName, 
            stream, request.ContentType);
        
        var message = CreateMessage(uri);
        await _publish.Publish(message, x =>
        {
            x.Serializer = new SystemTextJsonCustomRawMessageSerializer(RawSerializerOptions.All);
            x.MessageId = message.MessageId;
            x.CorrelationId = message.CorrelationId;
        }, cancellationToken);
    }

    private UserImageUploaded CreateMessage(Uri blobUri)
    {
        var imageUploaded = new UserImageUploadedBody
        {
            UserId = _currentUser.GetUserId(),
            ImageName = GetBlobName(),
            Uri = blobUri.ToString(),
            UserName = _currentUser.GetUserName()
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