using ImageMagick;
using User.Application.Interfaces;

namespace User.Application.Services;

public class MagickImageService : IImageService
{
    public async Task<Stream> ResizeImage(byte[] image, int width, int height)
    {
        using var imageStream = new MagickImage(image);

        var size = new MagickGeometry(width, height)
        {
            IgnoreAspectRatio = false
        };
        
        imageStream.Resize(size);
        var resizedStream = new MemoryStream();
        await imageStream.WriteAsync(resizedStream);
        resizedStream.Seek(0, SeekOrigin.Begin);
        return resizedStream;
    }
}