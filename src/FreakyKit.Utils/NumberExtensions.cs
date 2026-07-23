namespace FreakyKit.Utils;

public static class NumberExtensions
{
    /// <summary>
    /// Returns <c>true</c> when <paramref name="number"/> is within the inclusive range
    /// [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <typeparam name="T">Any numeric type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="number">The value to test.</param>
    /// <param name="min">Inclusive lower bound.</param>
    /// <param name="max">Inclusive upper bound.</param>
    public static bool IsBetween<T>(this T number, T min, T max) where T : INumber<T>
    {
        return number >= min && number <= max;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="number"/> is divisible by 2.
    /// </summary>
    /// <typeparam name="T">Any integer type implementing <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="number">Value to test.</param>
    public static bool IsEven<T>(this T number) where T : IBinaryInteger<T> =>
        number % (T.One + T.One) == T.Zero;

    /// <summary>
    /// Returns <c>true</c> when <paramref name="number"/> is not divisible by 2.
    /// </summary>
    /// <typeparam name="T">Any integer type implementing <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="number">Value to test.</param>
    public static bool IsOdd<T>(this T number) where T : IBinaryInteger<T> => !number.IsEven();

    /// <summary>
    /// Instance form of <c>T.Clamp(min, max)</c>: returns <paramref name="number"/> bounded to the inclusive
    /// range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <typeparam name="T">Any numeric type implementing <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="number">Value to clamp.</param>
    /// <param name="min">Inclusive lower bound.</param>
    /// <param name="max">Inclusive upper bound.</param>
    public static T Clamp<T>(this T number, T min, T max) where T : INumber<T> =>
        T.Clamp(number, min, max);

    /// <summary>
    /// Rounds <paramref name="value"/> to the supplied number of fractional digits using
    /// <see cref="Math.Round(double, int)"/> (banker's rounding).
    /// </summary>
    /// <param name="value">Value to round.</param>
    /// <param name="decimals">Number of fractional digits (0–15).</param>
    public static double RoundTo(this double value, int decimals) => Math.Round(value, decimals);

    /// <summary>
    /// Rounds <paramref name="value"/> to the supplied number of fractional digits.
    /// </summary>
    /// <param name="value">Value to round.</param>
    /// <param name="decimals">Number of fractional digits.</param>
    public static decimal RoundTo(this decimal value, int decimals) => Math.Round(value, decimals);

    /// <summary>
    /// Linearly remaps <paramref name="value"/> from the source range
    /// [<paramref name="fromMin"/>, <paramref name="fromMax"/>] to the target range
    /// [<paramref name="toMin"/>, <paramref name="toMax"/>].
    /// For integral types, arithmetic overflow throws <see cref="OverflowException"/>.
    /// </summary>
    /// <typeparam name="T">Any numeric type implementing <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">Value in the source range.</param>
    /// <param name="fromMin">Lower bound of the source range.</param>
    /// <param name="fromMax">Upper bound of the source range.</param>
    /// <param name="toMin">Lower bound of the target range.</param>
    /// <param name="toMax">Upper bound of the target range.</param>
    /// <exception cref="OverflowException">Thrown when arithmetic overflows for integral types.</exception>
    public static T Map<T>(this T value, T fromMin, T fromMax, T toMin, T toMax) where T : INumber<T>
    {
        if (fromMin == fromMax) throw new ArgumentException("Source range has zero width.");
        checked
        {
            return toMin + ((value - fromMin) * (toMax - toMin) / (fromMax - fromMin));
        }
    }
}