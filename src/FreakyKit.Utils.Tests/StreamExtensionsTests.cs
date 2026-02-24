namespace FreakyKit.Utils.Tests;

public class StreamExtensionsTests
{
    // A stream wrapper that is not a MemoryStream, used to exercise the non-MemoryStream path.
    private sealed class NonMemoryStream(Stream inner) : Stream
    {
        public override bool CanRead => inner.CanRead;
        public override bool CanSeek => inner.CanSeek;
        public override bool CanWrite => inner.CanWrite;
        public override long Length => inner.Length;
        public override long Position { get => inner.Position; set => inner.Position = value; }
        public override void Flush() => inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => inner.Seek(offset, origin);
        public override void SetLength(long value) => inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => inner.Write(buffer, offset, count);
    }

    // ------- GetMemoryStream -------

    [Fact]
    public void GetMemoryStream_ConvertsSourceStreamToMemoryStream()
    {
        byte[] data = [1, 2, 3, 4, 5];
        using var source = new MemoryStream(data);

        using var result = source.GetMemoryStream();

        Assert.IsType<MemoryStream>(result);
        Assert.Equal(data, result.ToArray());
    }

    [Fact]
    public void GetMemoryStream_PositionIsZeroAfterConversion()
    {
        byte[] data = [10, 20, 30];
        using var source = new MemoryStream(data);

        using var result = source.GetMemoryStream();

        Assert.Equal(0, result.Position);
    }

    // ------- GetBase64 -------

    [Fact]
    public void GetBase64_MemoryStream_ReturnsCorrectBase64()
    {
        byte[] data = [1, 2, 3];
        using var stream = new MemoryStream(data);

        var result = stream.GetBase64();

        Assert.Equal(Convert.ToBase64String(data), result);
    }

    [Fact]
    public void GetBase64_NonMemoryStream_ReturnsCorrectBase64()
    {
        byte[] data = [10, 20, 30, 40];
        using var inner = new MemoryStream(data);
        using var stream = new NonMemoryStream(inner);

        var result = stream.GetBase64();

        Assert.Equal(Convert.ToBase64String(data), result);
    }

    [Fact]
    public void GetBase64_NullStream_ReturnsNull()
    {
        Stream stream = null!;

        var result = stream.GetBase64();

        Assert.Null(result);
    }

    [Fact]
    public void GetBase64_EmptyStream_ReturnsEmptyBase64()
    {
        using var stream = new MemoryStream([]);

        var result = stream.GetBase64();

        Assert.Equal(Convert.ToBase64String([]), result);
    }
}
