namespace FreakyKit.Utils;

public static class DateTimeExtensions
{
    /// <summary>
    /// Returns <c>true</c> when <paramref name="date"/> falls on Monday through Friday.
    /// </summary>
    /// <param name="date">The date to test.</param>
    public static bool IsWeekDay(this DateTime date) =>
        date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

    /// <summary>
    /// Returns <c>true</c> when <paramref name="date"/> falls on Saturday or Sunday.
    /// </summary>
    /// <param name="date">The date to test.</param>
    public static bool IsWeekend(this DateTime date) =>
        date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

    /// <summary>
    /// Returns <paramref name="date"/> if it is already a weekday; otherwise advances one day at a time
    /// until the next Monday–Friday is reached.
    /// </summary>
    /// <remarks>
    /// Public holidays and locale-specific weekends (e.g. Fri/Sat) are not considered.
    /// </remarks>
    /// <param name="date">The starting date.</param>
    public static DateTime NextWorkday(this DateTime date)
    {
        var nextDay = date;
        while (!nextDay.IsWeekDay())
            nextDay = nextDay.AddDays(1);
        return nextDay;
    }

    /// <summary>
    /// Returns <paramref name="date"/> if it is already a weekday; otherwise rewinds one day at a time
    /// until the previous Monday–Friday is reached.
    /// </summary>
    /// <param name="date">The starting date.</param>
    public static DateTime PreviousWorkday(this DateTime date)
    {
        var prevDay = date;
        while (!prevDay.IsWeekDay())
            prevDay = prevDay.AddDays(-1);
        return prevDay;
    }

    /// <summary>
    /// Returns midnight (<c>00:00:00.0000000</c>) of the same calendar day, preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="date">Reference date.</param>
    public static DateTime StartOfDay(this DateTime date) =>
        new(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);

    /// <summary>
    /// Returns the last representable instant of the same calendar day (<c>23:59:59.9999999</c>),
    /// preserving <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="date">Reference date.</param>
    public static DateTime EndOfDay(this DateTime date) =>
        new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind).AddDays(1).AddTicks(-1);

    /// <summary>
    /// Returns <see cref="StartOfDay(DateTime)"/> of the first occurrence of <paramref name="firstDay"/>
    /// at or before <paramref name="date"/>. Defaults to Monday-aligned weeks.
    /// </summary>
    /// <param name="date">Reference date.</param>
    /// <param name="firstDay">Day considered as the start of the week (default <see cref="DayOfWeek.Monday"/>).</param>
    public static DateTime StartOfWeek(this DateTime date, DayOfWeek firstDay = DayOfWeek.Monday)
    {
        int diff = (7 + (date.DayOfWeek - firstDay)) % 7;
        return date.AddDays(-diff).StartOfDay();
    }

    /// <summary>
    /// Returns <see cref="EndOfDay(DateTime)"/> of the last day of the week that starts on <paramref name="firstDay"/>.
    /// </summary>
    /// <param name="date">Reference date.</param>
    /// <param name="firstDay">Day considered as the start of the week (default <see cref="DayOfWeek.Monday"/>).</param>
    public static DateTime EndOfWeek(this DateTime date, DayOfWeek firstDay = DayOfWeek.Monday) =>
        date.StartOfWeek(firstDay).AddDays(6).EndOfDay();

    /// <summary>
    /// Returns <see cref="StartOfDay(DateTime)"/> of the first day of the same month.
    /// </summary>
    /// <param name="date">Reference date.</param>
    public static DateTime StartOfMonth(this DateTime date) =>
        new(date.Year, date.Month, 1, 0, 0, 0, date.Kind);

    /// <summary>
    /// Returns <see cref="EndOfDay(DateTime)"/> of the last day of the same month.
    /// </summary>
    /// <param name="date">Reference date.</param>
    public static DateTime EndOfMonth(this DateTime date) =>
        new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 0, 0, 0, date.Kind).EndOfDay();

    /// <summary>
    /// Returns <c>true</c> when <paramref name="date"/> and <paramref name="other"/> fall on the same calendar day.
    /// Time-of-day is ignored.
    /// </summary>
    /// <param name="date">First date.</param>
    /// <param name="other">Second date.</param>
    public static bool IsSameDay(this DateTime date, DateTime other) =>
        date.Year == other.Year && date.Month == other.Month && date.Day == other.Day;

    /// <summary>
    /// Returns the whole-year difference between <paramref name="date"/> (treated as a birth/start date)
    /// and <paramref name="referenceDate"/> (default <see cref="DateTime.Today"/>).
    /// </summary>
    /// <param name="date">The earlier date (e.g. date of birth).</param>
    /// <param name="referenceDate">The later date to measure against. Defaults to today.</param>
    public static int Age(this DateTime date, DateTime? referenceDate = null)
    {
        var reference = referenceDate ?? DateTime.Today;
        int years = reference.Year - date.Year;
        if (reference.Month < date.Month || (reference.Month == date.Month && reference.Day < date.Day))
            years--;
        return years;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="date"/> falls on the current local date.
    /// </summary>
    public static bool IsToday(this DateTime date) => date.IsSameDay(DateTime.Today);

    /// <summary>
    /// Returns <c>true</c> when <paramref name="date"/> falls on yesterday's local date.
    /// </summary>
    public static bool IsYesterday(this DateTime date) => date.IsSameDay(DateTime.Today.AddDays(-1));

    /// <summary>
    /// Returns <c>true</c> when <paramref name="date"/> falls on tomorrow's local date.
    /// </summary>
    public static bool IsTomorrow(this DateTime date) => date.IsSameDay(DateTime.Today.AddDays(1));

    /// <summary>
    /// Converts <paramref name="date"/> to the number of seconds since the Unix epoch
    /// (<c>1970-01-01T00:00:00Z</c>) by way of <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="date">The date to convert.</param>
    public static long ToUnixTimeSeconds(this DateTime date) =>
        new DateTimeOffset(date, date.Kind == DateTimeKind.Utc ? TimeSpan.Zero : TimeZoneInfo.Local.GetUtcOffset(date)).ToUnixTimeSeconds();
}