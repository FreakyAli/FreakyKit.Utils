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

    [Theory]
    [InlineData(0, true)]
    [InlineData(2, true)]
    [InlineData(-4, true)]
    [InlineData(1, false)]
    [InlineData(-3, false)]
    public void IsEven_Int(int value, bool expected)
    {
        Assert.Equal(expected, value.IsEven());
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(-7, true)]
    [InlineData(0, false)]
    [InlineData(2, false)]
    public void IsOdd_Int(int value, bool expected)
    {
        Assert.Equal(expected, value.IsOdd());
    }

    [Fact]
    public void IsEven_Long()
    {
        Assert.True(1000L.IsEven());
        Assert.False(1001L.IsEven());
    }

    [Fact]
    public void Clamp_BelowMin_ReturnsMin()
    {
        Assert.Equal(0, (-5).Clamp(0, 10));
    }

    [Fact]
    public void Clamp_AboveMax_ReturnsMax()
    {
        Assert.Equal(10, 99.Clamp(0, 10));
    }

    [Fact]
    public void Clamp_InRange_ReturnsValue()
    {
        Assert.Equal(7, 7.Clamp(0, 10));
    }

    [Fact]
    public void Clamp_Double()
    {
        Assert.Equal(1.0, 5.5.Clamp(0.0, 1.0));
    }

    [Fact]
    public void RoundTo_Double()
    {
        Assert.Equal(3.14, 3.14159.RoundTo(2));
    }

    [Fact]
    public void RoundTo_Decimal()
    {
        Assert.Equal(3.14m, 3.14159m.RoundTo(2));
    }

    [Fact]
    public void Map_LinearRemap()
    {
        // Map 5 from [0,10] to [0,100] => 50
        Assert.Equal(50.0, 5.0.Map(0.0, 10.0, 0.0, 100.0));
    }

    [Fact]
    public void Map_NegativeRange()
    {
        // Map 0 from [-1,1] to [0,100] => 50
        Assert.Equal(50.0, 0.0.Map(-1.0, 1.0, 0.0, 100.0));
    }

    [Fact]
    public void Map_ZeroSourceWidth_Throws()
    {
        Assert.Throws<ArgumentException>(() => 5.0.Map(1.0, 1.0, 0.0, 10.0));
    }

    [Fact]
    public void Map_IntegralOverflow_Throws()
    {
        // Map large int that causes arithmetic overflow
        Assert.Throws<OverflowException>(() =>
            int.MaxValue.Map(0, 1, 0, int.MaxValue));
    }
}
