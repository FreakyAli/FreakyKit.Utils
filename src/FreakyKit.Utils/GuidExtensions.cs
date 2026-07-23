namespace FreakyKit.Utils;

public static class GuidExtensions
{
    /// <summary>
    /// Encodes <paramref name="guid"/> as a 22-character URL-safe Base64 string (no padding).
    /// Inverse of <see cref="ParseShortGuid(string)"/>.
    /// </summary>
    /// <param name="guid">Guid to encode.</param>
    public static string ToShortString(this Guid guid)
    {
        var b64 = Convert.ToBase64String(guid.ToByteArray());
        // URL-safe + strip the two trailing '=' characters.
        return b64.Replace('+', '-').Replace('/', '_')[..22];
    }

    /// <summary>
    /// Decodes a 22-character short string (produced by <see cref="ToShortString(Guid)"/>) back to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">22-character short-form GUID.</param>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not a valid short GUID.</exception>
    public static Guid ParseShortGuid(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length != 22) throw new FormatException("Short GUID must be exactly 22 characters.");
        var b64 = value.Replace('-', '+').Replace('_', '/') + "==";
        var bytes = Convert.FromBase64String(b64);
        return new Guid(bytes);
    }

    /// <summary>
    /// Attempts to decode a 22-character short string back to a <see cref="Guid"/>. Returns <c>false</c>
    /// on any error.
    /// </summary>
    /// <param name="value">Candidate short-form GUID.</param>
    /// <param name="result">Parsed GUID on success; <see cref="Guid.Empty"/> on failure.</param>
    public static bool TryParseShortGuid(this string value, out Guid result)
    {
        result = Guid.Empty;
        if (value is null || value.Length != 22) return false;
        try
        {
            var b64 = value.Replace('-', '+').Replace('_', '/') + "==";
            var bytes = Convert.FromBase64String(b64);
            if (bytes.Length != 16) return false;
            result = new Guid(bytes);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="guid"/> equals <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="guid">Guid to test.</param>
    public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;
}
