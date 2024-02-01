using Microsoft.AspNetCore.Http;

namespace Dtos.Common;

public record FileUploadDto
{
    public IFormFile File { get; init; }  = default!;
}