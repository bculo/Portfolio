namespace Dtos.Common;

public record FileDetailsDto
{
    public byte[] Content { get; init; }
    public string ContentType { get; init; }
    public string Name { get; init; }
}