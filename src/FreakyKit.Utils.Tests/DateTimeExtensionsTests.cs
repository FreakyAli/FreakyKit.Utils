namespace FreakyKit.Utils.Tests;

public class DateTimeExtensionsTests
{
    // Reference week: 2024-01-15 (Mon) → 2024-01-19 (Fri), 2024-01-20 (Sat), 2024-01-21 (Sun)

    [Theory]
    [InlineData(2024, 1, 15)] // Monday
    [InlineData(2024, 1, 16)] // Tuesday
    [InlineData(2024, 1, 17)] // Wednesday
    [InlineData(2024, 1, 18)] // Thursday
    [InlineData(2024, 1, 19)] // Friday
    public void IsWeekDay_Weekday_ReturnsTrue(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        Assert.True(date.IsWeekDay());
    }

    [Theory]
    [InlineData(2024, 1, 20)] // Saturday
    [InlineData(2024, 1, 21)] // Sunday
    public void IsWeekDay_Weekend_ReturnsFalse(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        Assert.False(date.IsWeekDay());
    }

    [Theory]
    [InlineData(2024, 1, 20)] // Saturday
    [InlineData(2024, 1, 21)] // Sunday
    public void IsWeekend_Weekend_ReturnsTrue(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        Assert.True(date.IsWeekend());
    }

    [Theory]
    [InlineData(2024, 1, 15)] // Monday
    [InlineData(2024, 1, 17)] // Wednesday
    [InlineData(2024, 1, 19)] // Friday
    public void IsWeekend_Weekday_ReturnsFalse(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);

        Assert.False(date.IsWeekend());
    }

    [Fact]
    public void NextWorkday_GivenWeekday_ReturnsSameDay()
    {
        var monday = new DateTime(2024, 1, 15);

        var result = monday.NextWorkday();

        Assert.Equal(monday, result);
    }

    [Fact]
    public void NextWorkday_GivenSaturday_ReturnsFollowingMonday()
    {
        var saturday = new DateTime(2024, 1, 20);
        var expectedMonday = new DateTime(2024, 1, 22);

        var result = saturday.NextWorkday();

        Assert.Equal(expectedMonday, result);
    }

    [Fact]
    public void NextWorkday_GivenSunday_ReturnsFollowingMonday()
    {
        var sunday = new DateTime(2024, 1, 21);
        var expectedMonday = new DateTime(2024, 1, 22);

        var result = sunday.NextWorkday();

        Assert.Equal(expectedMonday, result);
    }
}
