namespace FreakyKit.Utils.Tests;

public class RandomExtensionsTests
{
    private enum Color { Red, Green, Blue }

    [Fact]
    public void NextBool_BothValuesProduced()
    {
        var rnd = new Random(42);
        var trues = 0;
        var falses = 0;
        for (int i = 0; i < 200; i++)
        {
            if (rnd.NextBool()) trues++; else falses++;
        }
        Assert.True(trues > 0);
        Assert.True(falses > 0);
    }

    [Fact]
    public void NextEnum_ReturnsValidValue()
    {
        var rnd = new Random(42);
        for (int i = 0; i < 50; i++)
        {
            var value = rnd.NextEnum<Color>();
            Assert.Contains(value, new[] { Color.Red, Color.Green, Color.Blue });
        }
    }

    [Fact]
    public void NextElement_ReturnsItemFromSource()
    {
        var rnd = new Random(42);
        var source = new[] { 10, 20, 30, 40 };

        for (int i = 0; i < 20; i++)
            Assert.Contains(rnd.NextElement(source), source);
    }

    [Fact]
    public void NextElement_EmptySource_Throws()
    {
        var rnd = new Random();

        Assert.Throws<ArgumentException>(() => rnd.NextElement(Array.Empty<int>()));
    }

    [Fact]
    public void NextString_DefaultAlphabet_LengthMatches()
    {
        var rnd = new Random(42);

        var s = rnd.NextString(20);

        Assert.Equal(20, s.Length);
        Assert.All(s, c => Assert.True(char.IsLetterOrDigit(c)));
    }

    [Fact]
    public void NextString_CustomAlphabet_OnlyContainsThoseChars()
    {
        var rnd = new Random(42);

        var s = rnd.NextString(50, "ab");

        Assert.Equal(50, s.Length);
        Assert.All(s, c => Assert.Contains(c, "ab"));
    }

    [Fact]
    public void NextString_Negative_Throws()
    {
        var rnd = new Random();

        Assert.Throws<ArgumentOutOfRangeException>(() => rnd.NextString(-1));
    }

    [Fact]
    public void NextString_EmptyAlphabet_Throws()
    {
        var rnd = new Random();

        Assert.Throws<ArgumentException>(() => rnd.NextString(5, ""));
    }
}
