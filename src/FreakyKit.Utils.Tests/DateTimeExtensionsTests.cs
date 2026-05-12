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

    [Fact]
    public void PreviousWorkday_GivenWeekday_ReturnsSameDay()
    {
        var wednesday = new DateTime(2024, 1, 17);

        Assert.Equal(wednesday, wednesday.PreviousWorkday());
    }

    [Fact]
    public void PreviousWorkday_GivenSaturday_ReturnsPriorFriday()
    {
        var saturday = new DateTime(2024, 1, 20);
        var expectedFriday = new DateTime(2024, 1, 19);

        Assert.Equal(expectedFriday, saturday.PreviousWorkday());
    }

    [Fact]
    public void PreviousWorkday_GivenSunday_ReturnsPriorFriday()
    {
        var sunday = new DateTime(2024, 1, 21);
        var expectedFriday = new DateTime(2024, 1, 19);

        Assert.Equal(expectedFriday, sunday.PreviousWorkday());
    }

    [Fact]
    public void StartOfDay_ResetsTimeComponent()
    {
        var dt = new DateTime(2024, 6, 7, 13, 45, 30, DateTimeKind.Local);

        var result = dt.StartOfDay();

        Assert.Equal(new DateTime(2024, 6, 7, 0, 0, 0, DateTimeKind.Local), result);
        Assert.Equal(DateTimeKind.Local, result.Kind);
    }

    [Fact]
    public void EndOfDay_SetsLastTick()
    {
        var dt = new DateTime(2024, 6, 7, 5, 0, 0, DateTimeKind.Utc);

        var result = dt.EndOfDay();

        Assert.Equal(2024, result.Year);
        Assert.Equal(6, result.Month);
        Assert.Equal(7, result.Day);
        Assert.Equal(23, result.Hour);
        Assert.Equal(59, result.Minute);
        Assert.Equal(59, result.Second);
        Assert.Equal(9999999, result.Ticks % TimeSpan.TicksPerSecond);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void StartOfWeek_DefaultsToMonday()
    {
        var wednesday = new DateTime(2024, 1, 17, 14, 30, 0);

        var result = wednesday.StartOfWeek();

        Assert.Equal(new DateTime(2024, 1, 15), result);
    }

    [Fact]
    public void StartOfWeek_RespectsFirstDay()
    {
        var wednesday = new DateTime(2024, 1, 17);

        Assert.Equal(new DateTime(2024, 1, 14), wednesday.StartOfWeek(DayOfWeek.Sunday));
    }

    [Fact]
    public void EndOfWeek_DefaultsToSunday()
    {
        var wednesday = new DateTime(2024, 1, 17);

        var result = wednesday.EndOfWeek();

        Assert.Equal(new DateTime(2024, 1, 21), result.Date);
        Assert.Equal(23, result.Hour);
    }

    [Fact]
    public void StartOfMonth_ReturnsFirst()
    {
        var dt = new DateTime(2024, 6, 20, 9, 0, 0);

        Assert.Equal(new DateTime(2024, 6, 1), dt.StartOfMonth());
    }

    [Fact]
    public void EndOfMonth_ReturnsLastDayEndOfDay()
    {
        var dt = new DateTime(2024, 2, 5); // 2024 is a leap year — Feb has 29 days

        var result = dt.EndOfMonth();

        Assert.Equal(29, result.Day);
        Assert.Equal(23, result.Hour);
    }

    [Fact]
    public void IsSameDay_SameCalendarDay_ReturnsTrue()
    {
        var a = new DateTime(2024, 6, 7, 1, 0, 0);
        var b = new DateTime(2024, 6, 7, 23, 0, 0);

        Assert.True(a.IsSameDay(b));
    }

    [Fact]
    public void IsSameDay_DifferentDay_ReturnsFalse()
    {
        var a = new DateTime(2024, 6, 7);
        var b = new DateTime(2024, 6, 8);

        Assert.False(a.IsSameDay(b));
    }

    [Fact]
    public void Age_BeforeBirthdayThisYear_DoesNotCountCurrentYear()
    {
        var dob = new DateTime(1990, 12, 31);
        var reference = new DateTime(2024, 6, 1);

        Assert.Equal(33, dob.Age(reference));
    }

    [Fact]
    public void Age_OnOrAfterBirthdayThisYear_CountsCurrentYear()
    {
        var dob = new DateTime(1990, 1, 1);
        var reference = new DateTime(2024, 6, 1);

        Assert.Equal(34, dob.Age(reference));
    }

    [Fact]
    public void Age_DefaultsToToday()
    {
        var dob = DateTime.Today.AddYears(-25);

        Assert.Equal(25, dob.Age());
    }

    [Fact]
    public void IsToday_ReturnsTrueForToday()
    {
        Assert.True(DateTime.Today.IsToday());
        Assert.True(DateTime.Now.IsToday());
    }

    [Fact]
    public void IsYesterday_ReturnsTrueForYesterday()
    {
        Assert.True(DateTime.Today.AddDays(-1).IsYesterday());
    }

    [Fact]
    public void IsTomorrow_ReturnsTrueForTomorrow()
    {
        Assert.True(DateTime.Today.AddDays(1).IsTomorrow());
    }

    [Fact]
    public void ToUnixTimeSeconds_EpochReturnsZero()
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        Assert.Equal(0L, epoch.ToUnixTimeSeconds());
    }

    [Fact]
    public void ToUnixTimeSeconds_KnownInstant()
    {
        var dt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // 2024-01-01T00:00:00Z = 1704067200
        Assert.Equal(1704067200L, dt.ToUnixTimeSeconds());
    }
}
