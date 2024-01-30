using Sqids;

namespace Stock.Application.Common.Extensions;

public static class SqidsEncoderExtensions
{
    public static int DecodeSingle(this SqidsEncoder<int> encoder, string id)
    {
        var decodedElements = encoder.Decode(id);
        return !decodedElements.Any() ? default : decodedElements.First();
    }
}