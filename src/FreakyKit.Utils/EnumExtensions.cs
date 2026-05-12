using System.ComponentModel;
using System.Reflection;

namespace FreakyKit.Utils;

public static class EnumExtensions
{
    /// <summary>
    /// Returns the value of <see cref="DescriptionAttribute"/> on <paramref name="value"/>, falling back to
    /// the enum member's name when no attribute is present.
    /// </summary>
    /// <param name="value">Enum value.</param>
    public static string GetDescription(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var name = value.ToString();
        var field = value.GetType().GetField(name);
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? name;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="value"/> is a defined member of its enum type.
    /// </summary>
    /// <param name="value">Enum value.</param>
    public static bool IsDefined(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Enum.IsDefined(value.GetType(), value);
    }

    /// <summary>
    /// Parses <paramref name="value"/> as a member of enum type <typeparamref name="TEnum"/>. Case-insensitive.
    /// </summary>
    /// <typeparam name="TEnum">Target enum type.</typeparam>
    /// <param name="value">String to parse.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> does not match any member.</exception>
    public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(value);
        return Enum.Parse<TEnum>(value, ignoreCase: true);
    }

    /// <summary>
    /// Attempts to parse <paramref name="value"/> as a member of enum type <typeparamref name="TEnum"/>.
    /// Case-insensitive. Returns <c>false</c> on failure.
    /// </summary>
    /// <typeparam name="TEnum">Target enum type.</typeparam>
    /// <param name="value">String to parse.</param>
    /// <param name="result">Parsed enum on success; default on failure.</param>
    public static bool TryToEnum<TEnum>(this string value, out TEnum result) where TEnum : struct, Enum
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }
        return Enum.TryParse(value, ignoreCase: true, out result);
    }
}
