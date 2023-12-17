namespace User.Application.Interfaces;

public interface IImageService
{ 
    Task<Stream> ResizeImage(byte[] image, int width, int height);
}