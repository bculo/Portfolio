using Microsoft.AspNetCore.Http;

namespace Dtos.Common.Extensions;

public static class FormFileExtensions
{
    public static async Task<T> ToDetailsDto<T>(this IFormFile file) where T : FileDetailsDto, new()
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        return new T
        {
            Content = stream.ToArray(),
            Name = file.FileName,
            ContentType = file.ContentType
        };
    }
} 