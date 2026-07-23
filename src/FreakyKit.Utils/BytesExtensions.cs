namespace FreakyKit.Utils;

public static class BytesExtensions
{
    /// <summary>
    /// Encodes <paramref name="bytes"/> as an uppercase hexadecimal string. Wraps <see cref="Convert.ToHexString(byte[])"/>.
    /// </summary>
    /// <param name="bytes">Bytes to encode.</param>
    public static string ToHex(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        return Convert.ToHexString(bytes);
    }

    /// <summary>
    /// Decodes a hexadecimal string into a byte array. Wraps <see cref="Convert.FromHexString(string)"/>.
    /// </summary>
    /// <param name="hex">Hex string (case-insensitive, even length).</param>
    /// <exception cref="FormatException">Thrown when <paramref name="hex"/> is not valid hex.</exception>
    public static byte[] FromHex(this string hex)
    {
        ArgumentNullException.ThrowIfNull(hex);
        return Convert.FromHexString(hex);
    }

    /// <summary>
    /// Encodes <paramref name="bytes"/> as a Base64 string.
    /// </summary>
    /// <param name="bytes">Bytes to encode.</param>
    public static string ToBase64(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Decodes <paramref name="bytes"/> as a string using the supplied <paramref name="encoding"/>
    /// (default: <see cref="Encoding.UTF8"/>).
    /// </summary>
    /// <param name="bytes">Bytes to decode.</param>
    /// <param name="encoding">Encoding to use. Defaults to UTF-8.</param>
    public static string AsString(this byte[] bytes, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        return (encoding ?? Encoding.UTF8).GetString(bytes);
    }
}
