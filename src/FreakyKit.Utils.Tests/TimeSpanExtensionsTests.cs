namespace FreakyKit.Utils.Tests;

public class TimeSpanExtensionsTests
{
    [Fact]
    public void Min_ReturnsSmaller()
    {
        var a = TimeSpan.FromMinutes(10);
        var b = TimeSpan.FromMinutes(20);

        Assert.Equal(a, a.Min(b));
        Assert.Equal(a, b.Min(a));
    }

    [Fact]
    public void Max_ReturnsLarger()
    {
        var a = TimeSpan.FromMinutes(10);
        var b = TimeSpan.FromMinutes(20);

        Assert.Equal(b, a.Max(b));
        Assert.Equal(b, b.Max(a));
    }

    [Theory]
    [InlineData(0, 0, 0, 0, "0s")]
    [InlineData(0, 0, 0, 45, "45s")]
    [InlineData(0, 0, 5, 0, "5m")]
    [InlineData(0, 0, 5, 30, "5m 30s")]
    [InlineData(0, 1, 30, 0, "1h 30m")]
    [InlineData(0, 1, 0, 0, "1h")]
    [InlineData(2, 5, 0, 0, "2d 5h")]
    [InlineData(2, 0, 30, 0, "2d 30m")] // skips zero-hour and picks next non-zero unit
    public void ToHumanString_Formats(int days, int hours, int minutes, int seconds, string expected)
    {
        var span = new TimeSpan(days, hours, minutes, seconds);

        Assert.Equal(expected, span.ToHumanString());
    }

    [Fact]
    public void ToHumanString_Milliseconds()
    {
        Assert.Equal("250ms", TimeSpan.FromMilliseconds(250).ToHumanString());
    }

    [Fact]
    public void ToHumanString_Negative_HasMinusPrefix()
    {
        Assert.Equal("-1h 30m", TimeSpan.FromMinutes(-90).ToHumanString());
    }

    [Fact]
    public void ToHumanString_MinValue_DoesNotThrow()
    {
        // TimeSpan.MinValue.Duration() would overflow, so we handle it specially
        var result = TimeSpan.MinValue.ToHumanString();

        // Should have a minus prefix and some reasonable output without throwing
        Assert.StartsWith("-", result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void ToHumanString_OneTick_ShowsTickPrecision()
    {
        var result = TimeSpan.FromTicks(1).ToHumanString();

        // Should show tick precision, not "0ms"
        Assert.NotEqual("0s", result);
        Assert.Contains("tick", result);
    }
}
