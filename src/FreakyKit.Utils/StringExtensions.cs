using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FreakyKit.Utils;

public static partial class StringExtensions
{
    /// <summary>
    /// Decodes a Base64 string to its UTF-8 representation. Auto-pads with <c>=</c> when the
    /// length is not a multiple of four.
    /// </summary>
    /// <param name="value">Base64-encoded input.</param>
    public static string FromBase64(this string value)
    {
        while ((value.Length % 4) != 0)
        {
            value += "=";
        }

        byte[] decoded = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(decoded);
    }

    /// <summary>
    /// Encodes <paramref name="value"/> (interpreted as UTF-8) to a Base64 string.
    /// </summary>
    /// <param name="value">UTF-8 plain-text input.</param>
    public static string ToBase64(this string value)
    {
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytesToEncode);
    }

    /// <summary>
    /// Removes every character of <paramref name="value"/> that matches the supplied regex pattern.
    /// </summary>
    /// <param name="value">Input string.</param>
    /// <param name="allowedCharactersRegEx">Regex matching characters that should be stripped.</param>
    public static string RemoveUnwantedCharacters(this string value, string allowedCharactersRegEx)
    {
        return Regex.Replace(value, allowedCharactersRegEx, string.Empty);
    }

    [GeneratedRegex(@"[^0-9a-zA-Z-_.]")]
    private static partial Regex SpecialCharactersRegex();

    /// <summary>
    /// Strips every character outside the set <c>0-9 a-z A-Z - _ .</c> from <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Input string.</param>
    public static string RemoveSpecialCharacters(this string value)
    {
        return SpecialCharactersRegex().Replace(value, string.Empty);
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="value"/> contains only ASCII letters and digits
    /// (matches <c>^[a-zA-Z0-9]*$</c>). Empty strings also pass.
    /// </summary>
    /// <param name="value">String to test.</param>
    public static bool IsAlphaNumeric(this string value)
    {
        var pattern = "^[a-zA-Z0-9]*$";
        return Regex.IsMatch(value, pattern);
    }

    /// <summary>
    /// Formats <paramref name="value"/> as a currency string using the supplied culture.
    /// </summary>
    /// <param name="value">Numeric value to format.</param>
    /// <param name="cultureName">Culture identifier (e.g. <c>"en-US"</c>, <c>"de-DE"</c>).</param>
    public static string ToCurrency(this double value, string cultureName)
    {
        CultureInfo currentCulture = new(cultureName);
        return string.Format(currentCulture, "{0:C}", value);
    }

    /// <summary>
    /// Reverse a String
    /// </summary>
    /// <param name="input">The string to Reverse</param>
    /// <returns>The reversed String</returns>
    public static string Reverse(this string input)
    {
        char[] array = input.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="email"/> is accepted by <see cref="System.Net.Mail.MailAddress"/>.
    /// Lenient — only checks that the string is parseable as a mail address.
    /// </summary>
    /// <param name="email">Address to validate.</param>
    public static bool IsValidEmail(this string email)
    {
        var isValid = true;

        try
        {
            var emailAddress = new MailAddress(email.Trim());
        }
        catch
        {
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// Returns the first <paramref name="maxLength"/> characters of <paramref name="value"/>; if the input is longer,
    /// the result is suffixed with <paramref name="ellipsis"/>. Returns the input unchanged when it already fits.
    /// </summary>
    /// <param name="value">String to truncate.</param>
    /// <param name="maxLength">Maximum length (must be non-negative).</param>
    /// <param name="ellipsis">Suffix appended when truncation occurs. Defaults to <c>"…"</c>.</param>
    public static string Truncate(this string value, int maxLength, string ellipsis = "…")
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(maxLength);
        if (value.Length <= maxLength) return value;
        if (maxLength == 0) return string.Empty;
        return value[..maxLength] + ellipsis;
    }

    /// <summary>
    /// Returns <paramref name="value"/> concatenated with itself <paramref name="count"/> times.
    /// </summary>
    /// <param name="value">String to repeat.</param>
    /// <param name="count">Number of repetitions; must be non-negative.</param>
    public static string Repeat(this string value, int count)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        if (count == 0 || value.Length == 0) return string.Empty;
        var sb = new StringBuilder(value.Length * count);
        for (int i = 0; i < count; i++) sb.Append(value);
        return sb.ToString();
    }

    /// <summary>
    /// Returns the first <paramref name="count"/> characters of <paramref name="value"/>. If <paramref name="count"/>
    /// exceeds the length, the full string is returned. Negative counts throw.
    /// </summary>
    /// <param name="value">Input string.</param>
    /// <param name="count">Maximum characters to take from the left.</param>
    public static string Left(this string value, int count)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        return count >= value.Length ? value : value[..count];
    }

    /// <summary>
    /// Returns the last <paramref name="count"/> characters of <paramref name="value"/>. If <paramref name="count"/>
    /// exceeds the length, the full string is returned. Negative counts throw.
    /// </summary>
    /// <param name="value">Input string.</param>
    /// <param name="count">Maximum characters to take from the right.</param>
    public static string Right(this string value, int count)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        return count >= value.Length ? value : value[^count..];
    }

    /// <summary>
    /// Returns <paramref name="value"/> with every <see cref="char.IsWhiteSpace(char)"/> character removed.
    /// </summary>
    /// <param name="value">Input string.</param>
    public static string RemoveWhitespace(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        var sb = new StringBuilder(value.Length);
        foreach (var ch in value)
            if (!char.IsWhiteSpace(ch)) sb.Append(ch);
        return sb.ToString();
    }

    /// <summary>
    /// Splits <paramref name="value"/> into lines on any of <c>\r\n</c>, <c>\n</c>, or <c>\r</c>.
    /// Empty entries are preserved so a trailing newline produces an empty final entry.
    /// </summary>
    /// <param name="value">Input string.</param>
    public static string[] SplitLines(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Split(["\r\n", "\n", "\r"], StringSplitOptions.None);
    }

    /// <summary>
    /// Returns <paramref name="value"/> prefixed with <paramref name="prefix"/> only when it does not already start with it.
    /// </summary>
    /// <param name="value">Input string.</param>
    /// <param name="prefix">Prefix to ensure.</param>
    public static string EnsurePrefix(this string value, string prefix)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(prefix);
        return value.StartsWith(prefix, StringComparison.Ordinal) ? value : prefix + value;
    }

    /// <summary>
    /// Returns <paramref name="value"/> suffixed with <paramref name="suffix"/> only when it does not already end with it.
    /// </summary>
    /// <param name="value">Input string.</param>
    /// <param name="suffix">Suffix to ensure.</param>
    public static string EnsureSuffix(this string value, string suffix)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(suffix);
        return value.EndsWith(suffix, StringComparison.Ordinal) ? value : value + suffix;
    }

    /// <summary>
    /// Case-insensitive (ordinal) variant of <see cref="string.Contains(string)"/>.
    /// </summary>
    /// <param name="value">Source string.</param>
    /// <param name="other">Substring to search for.</param>
    public static bool ContainsIgnoreCase(this string value, string other) =>
        value.Contains(other, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Case-insensitive (ordinal) string equality.
    /// </summary>
    /// <param name="value">Source string.</param>
    /// <param name="other">String to compare against.</param>
    public static bool EqualsIgnoreCase(this string value, string other) =>
        string.Equals(value, other, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Case-insensitive (ordinal) variant of <see cref="string.StartsWith(string)"/>.
    /// </summary>
    /// <param name="value">Source string.</param>
    /// <param name="other">Prefix to test for.</param>
    public static bool StartsWithIgnoreCase(this string value, string other) =>
        value.StartsWith(other, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Case-insensitive (ordinal) variant of <see cref="string.EndsWith(string)"/>.
    /// </summary>
    /// <param name="value">Source string.</param>
    /// <param name="other">Suffix to test for.</param>
    public static bool EndsWithIgnoreCase(this string value, string other) =>
        value.EndsWith(other, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns <c>true</c> when <paramref name="value"/> parses as a <see cref="Guid"/> via <see cref="Guid.TryParse(string?, out Guid)"/>.
    /// </summary>
    /// <param name="value">String to validate.</param>
    public static bool IsValidGuid(this string value) => Guid.TryParse(value, out _);

    /// <summary>
    /// Returns <c>true</c> when <paramref name="value"/> parses as an absolute http/https URL.
    /// </summary>
    /// <param name="value">String to validate.</param>
    public static bool IsValidUrl(this string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}