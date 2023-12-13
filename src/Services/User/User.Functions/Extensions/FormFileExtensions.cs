using Microsoft.AspNetCore.Http;

namespace User.Functions.Extensions;

public static class FormFileExtensions
{
    public static async Task<byte[]> ToBytes(this IFormFile file)
    {
        await using var fileStream = file.OpenReadStream();
        var fileBytes = new byte[file.Length];
        var _ = await fileStream.ReadAsync(fileBytes, 0, (int)file.Length);
        return fileBytes;
    }
}