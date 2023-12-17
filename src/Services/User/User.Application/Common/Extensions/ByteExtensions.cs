namespace User.Application.Common.Extensions;

public static class ByteExtensions
{
    public static Stream ToStream(this byte[] array)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        return new MemoryStream(array);
    }
}