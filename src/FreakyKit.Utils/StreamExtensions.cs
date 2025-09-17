namespace FreakyKit.Utils;

public static class StreamExtensions
{
    public static MemoryStream GetMemoryStream(this Stream stream)
    {
        MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public static string? GetBase64(this Stream stream)
    {
        if (stream == null)
        {
            return null;
        }

        if (stream is MemoryStream memStream)
        {
            var allBytes = memStream.ToArray();
            return Convert.ToBase64String(allBytes);
        }

        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
        }
        return Convert.ToBase64String(bytes);
    }
}