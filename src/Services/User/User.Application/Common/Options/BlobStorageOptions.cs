namespace User.Application.Common.Options;

public class BlobStorageOptions
{
    public string VerificationContainerName { get; set; }
    public string ProfileContainerName { get; set; }
    public string ConnectionString { get; set; }
}