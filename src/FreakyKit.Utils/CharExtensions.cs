namespace FreakyKit.Utils;

public static class CharExtensions
{
    /// <summary>
    /// Returns <c>true</c> when <paramref name="ch"/> is an English vowel (a, e, i, o, u), case-insensitive.
    /// </summary>
    /// <param name="ch">Character to test.</param>
    public static bool IsVowel(this char ch) => "aeiouAEIOU".Contains(ch);

    /// <summary>
    /// Returns <c>true</c> when <paramref name="ch"/> is an English consonant (letter, not a vowel).
    /// </summary>
    /// <param name="ch">Character to test.</param>
    public static bool IsConsonant(this char ch) => char.IsLetter(ch) && !ch.IsVowel();

    /// <summary>
    /// Returns a string consisting of <paramref name="ch"/> repeated <paramref name="count"/> times.
    /// </summary>
    /// <param name="ch">Character to repeat.</param>
    /// <param name="count">Number of repetitions (must be non-negative).</param>
    public static string Repeat(this char ch, int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        return count == 0 ? string.Empty : new string(ch, count);
    }
}
