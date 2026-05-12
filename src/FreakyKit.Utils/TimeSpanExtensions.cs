namespace FreakyKit.Utils;

public static class TimeSpanExtensions
{
    /// <summary>
    /// Returns the smaller of <paramref name="span"/> and <paramref name="other"/>.
    /// </summary>
    public static TimeSpan Min(this TimeSpan span, TimeSpan other) =>
        span <= other ? span : other;

    /// <summary>
    /// Returns the larger of <paramref name="span"/> and <paramref name="other"/>.
    /// </summary>
    public static TimeSpan Max(this TimeSpan span, TimeSpan other) =>
        span >= other ? span : other;

    /// <summary>
    /// Returns a compact human-readable representation of <paramref name="span"/>, picking the two most
    /// significant non-zero units. Examples: <c>"1h 30m"</c>, <c>"45s"</c>, <c>"2d 5h"</c>, <c>"0s"</c>.
    /// Negative spans are prefixed with <c>"-"</c>.
    /// </summary>
    /// <param name="span">Duration to format.</param>
    public static string ToHumanString(this TimeSpan span)
    {
        if (span == TimeSpan.Zero) return "0s";

        var sign = span < TimeSpan.Zero ? "-" : "";
        var abs = span.Duration();

        var parts = new List<string>(2);
        if (abs.Days > 0) parts.Add($"{abs.Days}d");
        if (abs.Hours > 0) parts.Add($"{abs.Hours}h");
        if (parts.Count < 2 && abs.Minutes > 0) parts.Add($"{abs.Minutes}m");
        if (parts.Count < 2 && abs.Seconds > 0) parts.Add($"{abs.Seconds}s");
        if (parts.Count == 0) parts.Add($"{abs.Milliseconds}ms");

        return sign + string.Join(" ", parts.Take(2));
    }
}
