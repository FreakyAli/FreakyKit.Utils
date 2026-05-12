using System.Text;

namespace FreakyKit.Utils.Tests;

public class BytesExtensionsTests
{
    [Fact]
    public void ToHex_KnownInput()
    {
        var bytes = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };

        Assert.Equal("DEADBEEF", bytes.ToHex());
    }

    [Fact]
    public void ToHex_Empty_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, Array.Empty<byte>().ToHex());
    }

    [Fact]
    public void FromHex_RoundTrip()
    {
        var bytes = new byte[] { 1, 2, 3, 255 };

        Assert.Equal(bytes, bytes.ToHex().FromHex());
    }

    [Fact]
    public void FromHex_LowerCase()
    {
        Assert.Equal(new byte[] { 0xab, 0xcd }, "abcd".FromHex());
    }

    [Fact]
    public void FromHex_InvalidLength_Throws()
    {
        Assert.Throws<FormatException>(() => "abc".FromHex());
    }

    [Fact]
    public void ToBase64_KnownInput()
    {
        var bytes = Encoding.UTF8.GetBytes("hello");

        Assert.Equal("aGVsbG8=", bytes.ToBase64());
    }

    [Fact]
    public void AsString_DefaultUtf8()
    {
        var bytes = Encoding.UTF8.GetBytes("hello");

        Assert.Equal("hello", bytes.AsString());
    }

    [Fact]
    public void AsString_CustomEncoding()
    {
        var bytes = Encoding.Unicode.GetBytes("hi");

        Assert.Equal("hi", bytes.AsString(Encoding.Unicode));
    }
}
