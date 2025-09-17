namespace FreakyKit.Utils;

public static class NumberExtensions
{
    public static bool IsBetween<T>(this T number, T min, T max) where T : INumber<T>
    {   
        return number >= min && number <= max;
    }
}