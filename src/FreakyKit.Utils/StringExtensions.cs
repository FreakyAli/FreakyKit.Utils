using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FreakyKit.Utils;

public static partial class StringExtensions
{
    public static string FromBase64(this string value)
    {
        while ((value.Length % 4) != 0)
        {
            value += "=";
        }

        byte[] decoded = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(decoded);
    }

    public static string ToBase64(this string value)
    {
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytesToEncode);
    }

    public static string RemoveUnwantedCharacters(this string value, string allowedCharactersRegEx)
    {
        return Regex.Replace(value, allowedCharactersRegEx, string.Empty);
    }

    [GeneratedRegex(@"[^0-9a-zA-Z-_.]")]
    private static partial Regex SpecialCharactersRegex();

    public static string RemoveSpecialCharacters(this string value)
    {
        return SpecialCharactersRegex().Replace(value, string.Empty);
    }

    public static bool IsAlphaNumeric(this string value)
    {
        var pattern = "^[a-zA-Z0-9]*$";
        return Regex.IsMatch(value, pattern);
    }

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
}