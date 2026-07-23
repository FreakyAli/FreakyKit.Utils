namespace FreakyKit.Utils;

public static class StreamExtensions
{
    /// <summary>
    /// Copies the contents of <paramref name="stream"/> into a new <see cref="MemoryStream"/> and rewinds
    /// the result to position 0.
    /// </summary>
    /// <param name="stream">Source stream to buffer.</param>
    public static MemoryStream GetMemoryStream(this Stream stream)
    {
        MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    /// <summary>
    /// Returns the contents of <paramref name="stream"/> as a Base64-encoded string, starting from the
    /// current <see cref="Stream.Position"/> to <see cref="Stream.Length"/>. Uses <see cref="MemoryStream.ToArray"/>
    /// when possible to avoid an extra copy. Returns <c>null</c> when <paramref name="stream"/> is <c>null</c>.
    /// </summary>
    /// <param name="stream">Stream whose remaining bytes (from Position to end) should be encoded.</param>
    public static string? GetBase64(this Stream stream)
    {
        if (stream == null)
        {
            return null;
        }

        if (stream is MemoryStream memStream)
        {
            int start = (int)memStream.Position;
            int count = (int)(memStream.Length - memStream.Position);
            if (memStream.TryGetBuffer(out var seg))
                return Convert.ToBase64String(seg.Array!, seg.Offset + start, count);
            byte[] memBytes = memStream.ToArray();
            return Convert.ToBase64String(memBytes, start, count);
        }

        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
        }
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Fully reads <paramref name="stream"/> into a new byte array. Fast-path uses <see cref="MemoryStream.ToArray"/>.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    public static byte[] ToByteArray(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (stream is MemoryStream ms) return ms.ToArray();

        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    /// <summary>
    /// Asynchronously reads the entire <paramref name="stream"/> into a new byte array.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="token">Cancellation token.</param>
    public static async Task<byte[]> ReadAllBytesAsync(this Stream stream, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (stream is MemoryStream ms) return ms.ToArray();

        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, token).ConfigureAwait(false);
        return memory.ToArray();
    }
}