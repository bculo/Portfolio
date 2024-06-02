namespace User.Application.Common.Options;

public class BlobStorageOptions
{
    public string VerificationContainerName { get; set; } = default!;
    public string ProfileContainerName { get; set; } = default!;
    public string ConnectionString { get; set; }  = default!;
}