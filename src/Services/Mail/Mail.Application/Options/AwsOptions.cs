namespace Mail.Application.Options;

public sealed class AwsOptions
{
    public string AccessKeyId { get; set; }
    public string AccessKeySecret { get; set; }
    public string Region { get; set; }
    public string ServiceUrl { get; set; }
}