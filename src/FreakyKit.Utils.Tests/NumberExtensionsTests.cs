namespace FreakyKit.Utils.Tests;

public class NumberExtensionsTests
{
    [Theory]
    [InlineData(5, 1, 10, true)]
    [InlineData(1, 1, 10, true)]   // exactly at min
    [InlineData(10, 1, 10, true)]  // exactly at max
    [InlineData(0, 1, 10, false)]  // below min
    [InlineData(11, 1, 10, false)] // above max
    public void IsBetween_Int_ReturnsExpectedResult(int number, int min, int max, bool expected)
    {
        Assert.Equal(expected, number.IsBetween(min, max));
    }

    [Theory]
    [InlineData(3.14, 0.0, 10.0, true)]
    [InlineData(0.0, 0.0, 10.0, true)]   // exactly at min
    [InlineData(10.0, 0.0, 10.0, true)]  // exactly at max
    [InlineData(-0.1, 0.0, 10.0, false)]
    [InlineData(10.1, 0.0, 10.0, false)]
    public void IsBetween_Double_ReturnsExpectedResult(double number, double min, double max, bool expected)
    {
        Assert.Equal(expected, number.IsBetween(min, max));
    }

    [Fact]
    public void IsBetween_Long_ReturnsCorrectResult()
    {
        long value = 500L;

        Assert.True(value.IsBetween(100L, 1000L));
        Assert.False(value.IsBetween(600L, 1000L));
    }

    [Fact]
    public void IsBetween_MinEqualsMax_OnlyTrueWhenEqual()
    {
        Assert.True(5.IsBetween(5, 5));
        Assert.False(4.IsBetween(5, 5));
    }
}
