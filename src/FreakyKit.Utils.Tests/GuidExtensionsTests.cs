namespace FreakyKit.Utils.Tests;

public class GuidExtensionsTests
{
    [Fact]
    public void ToShortString_Length22()
    {
        var guid = Guid.NewGuid();

        var s = guid.ToShortString();

        Assert.Equal(22, s.Length);
    }

    [Fact]
    public void ToShortString_UrlSafe()
    {
        // Generate a few GUIDs and verify none of the output contains + / or =
        for (int i = 0; i < 50; i++)
        {
            var s = Guid.NewGuid().ToShortString();
            Assert.DoesNotContain('+', s);
            Assert.DoesNotContain('/', s);
            Assert.DoesNotContain('=', s);
        }
    }

    [Fact]
    public void ToShortString_RoundTrip()
    {
        var guid = Guid.NewGuid();

        var s = guid.ToShortString();
        var back = s.ParseShortGuid();

        Assert.Equal(guid, back);
    }

    [Fact]
    public void ParseShortGuid_InvalidLength_Throws()
    {
        Assert.Throws<FormatException>(() => "tooshort".ParseShortGuid());
    }

    [Fact]
    public void ParseShortGuid_InvalidChars_Throws()
    {
        var bad = new string('!', 22);

        Assert.Throws<FormatException>(() => bad.ParseShortGuid());
    }

    [Fact]
    public void TryParseShortGuid_Valid()
    {
        var guid = Guid.NewGuid();
        var s = guid.ToShortString();

        Assert.True(s.TryParseShortGuid(out var parsed));
        Assert.Equal(guid, parsed);
    }

    [Fact]
    public void TryParseShortGuid_Invalid_ReturnsFalse()
    {
        Assert.False("nope".TryParseShortGuid(out var parsed));
        Assert.Equal(Guid.Empty, parsed);
    }

    [Fact]
    public void TryParseShortGuid_Null_ReturnsFalse()
    {
        Assert.False(((string?)null!).TryParseShortGuid(out var parsed));
        Assert.Equal(Guid.Empty, parsed);
    }

    [Fact]
    public void IsEmpty_EmptyGuid_True()
    {
        Assert.True(Guid.Empty.IsEmpty());
    }

    [Fact]
    public void IsEmpty_NonEmpty_False()
    {
        Assert.False(Guid.NewGuid().IsEmpty());
    }
}
