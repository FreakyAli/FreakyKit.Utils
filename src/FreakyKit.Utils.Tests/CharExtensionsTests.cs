namespace FreakyKit.Utils.Tests;

public class CharExtensionsTests
{
    [Theory]
    [InlineData('a', true)]
    [InlineData('E', true)]
    [InlineData('u', true)]
    [InlineData('b', false)]
    [InlineData('Z', false)]
    [InlineData('1', false)]
    public void IsVowel(char ch, bool expected)
    {
        Assert.Equal(expected, ch.IsVowel());
    }

    [Theory]
    [InlineData('b', true)]
    [InlineData('Z', true)]
    [InlineData('a', false)]
    [InlineData('1', false)]
    [InlineData(' ', false)]
    public void IsConsonant(char ch, bool expected)
    {
        Assert.Equal(expected, ch.IsConsonant());
    }

    [Fact]
    public void Repeat_Three()
    {
        Assert.Equal("---", '-'.Repeat(3));
    }

    [Fact]
    public void Repeat_Zero_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, 'x'.Repeat(0));
    }

    [Fact]
    public void Repeat_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 'x'.Repeat(-1));
    }
}
