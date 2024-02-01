using Microsoft.AspNetCore.Http;

namespace Dtos.Common;

public record FileCollectionUploadDto
{
    public IFormCollection Files { get; init; } = default!;
}