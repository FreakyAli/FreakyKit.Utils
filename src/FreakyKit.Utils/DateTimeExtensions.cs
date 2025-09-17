namespace FreakyKit.Utils;

public static class DateTimeExtensions
{
    public static bool IsWeekDay(this DateTime date) =>
        date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

    public static bool IsWeekend(this DateTime date) =>
        date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

    public static DateTime NextWorkday(this DateTime date)
    {
        var nextDay = date;
        while (!nextDay.IsWeekDay())
            nextDay = nextDay.AddDays(1);
        return nextDay;
    }
}