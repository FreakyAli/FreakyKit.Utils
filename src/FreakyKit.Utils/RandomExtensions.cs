namespace FreakyKit.Utils;

public static class RandomExtensions
{
    private const string DefaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    /// <summary>
    /// Returns a random boolean with 50/50 distribution.
    /// </summary>
    /// <param name="random">Random source.</param>
    public static bool NextBool(this Random random)
    {
        ArgumentNullException.ThrowIfNull(random);
        return random.Next(2) == 0;
    }

    /// <summary>
    /// Returns a uniformly random value of the enum type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Enum type.</typeparam>
    /// <param name="random">Random source.</param>
    public static T NextEnum<T>(this Random random) where T : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(random);
        var values = Enum.GetValues<T>();
        return values[random.Next(values.Length)];
    }

    /// <summary>
    /// Returns a uniformly random element from <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="random">Random source.</param>
    /// <param name="source">Non-empty collection.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="source"/> is empty.</exception>
    public static T NextElement<T>(this Random random, IReadOnlyList<T> source)
    {
        ArgumentNullException.ThrowIfNull(random);
        ArgumentNullException.ThrowIfNull(source);
        if (source.Count == 0) throw new ArgumentException("Source is empty.", nameof(source));
        return source[random.Next(source.Count)];
    }

    /// <summary>
    /// Returns a random string of length <paramref name="length"/> drawn from <paramref name="alphabet"/>
    /// (default: A–Z, a–z, 0–9).
    /// </summary>
    /// <param name="random">Random source.</param>
    /// <param name="length">Desired length (must be non-negative).</param>
    /// <param name="alphabet">Characters to draw from. Defaults to alphanumeric.</param>
    public static string NextString(this Random random, int length, string? alphabet = null)
    {
        ArgumentNullException.ThrowIfNull(random);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        var chars = alphabet ?? DefaultAlphabet;
        if (chars.Length == 0) throw new ArgumentException("Alphabet must be non-empty.", nameof(alphabet));

        var buffer = new char[length];
        for (int i = 0; i < length; i++)
            buffer[i] = chars[random.Next(chars.Length)];
        return new string(buffer);
    }
}
