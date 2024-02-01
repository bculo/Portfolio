namespace Dtos.Common;

public record FileDetailsDto
{
    public byte[] Content { get; init; }  = default!;
    public string ContentType { get; init; }  = default!;
    public string Name { get; init; }  = default!;
}