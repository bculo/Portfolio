namespace Mail.Application.Options;

public sealed class AwsOptions
{
    public string AccessKeyId { get; set; } = default!;
    public string AccessKeySecret { get; set; } = default!;
    public string Region { get; set; } = default!;
    public string ServiceUrl { get; set; } = default!;
}