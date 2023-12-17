namespace User.Application.Interfaces;

public interface IBlobStorage
{
    Task<Uri> UploadBlob(string containerName, string blobIdentifier, byte[] blob,  string contentType);
}