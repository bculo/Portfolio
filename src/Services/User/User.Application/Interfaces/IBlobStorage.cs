namespace User.Application.Interfaces;

public interface IBlobStorage
{
    Task<Uri> UploadBlob(string containerName, string blobIdentifier, Stream blob,  string contentType);
}