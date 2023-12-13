namespace User.Application.Interfaces;

public interface IBlobStorage
{
    Task UploadBlob(string blobIdentifier, byte[] blob,  string contentType);
}