namespace FreakyKit.Utils.Tests;

public class StringExtensionsTests
{
    // ------- ToBase64 / FromBase64 -------

    [Fact]
    public void ToBase64_EncodesStringToBase64()
    {
        var result = "Hello".ToBase64();

        Assert.Equal("SGVsbG8=", result);
    }

    [Fact]
    public void FromBase64_DecodesBase64ToString()
    {
        var result = "SGVsbG8=".FromBase64();

        Assert.Equal("Hello", result);
    }

    [Fact]
    public void ToBase64_FromBase64_Roundtrip()
    {
        var original = "Hello, World! 123 🎉";

        var result = original.ToBase64().FromBase64();

        Assert.Equal(original, result);
    }

    [Fact]
    public void FromBase64_HandlesMissingPadding()
    {
        // "Hello" in base64 is "SGVsbG8=" — strip the padding and FromBase64 should restore it
        var result = "SGVsbG8".FromBase64();

        Assert.Equal("Hello", result);
    }

    // ------- RemoveUnwantedCharacters -------

    [Fact]
    public void RemoveUnwantedCharacters_RemovesCharactersMatchingPattern()
    {
        var result = "Hello123!@#".RemoveUnwantedCharacters(@"[^a-zA-Z0-9]");

        Assert.Equal("Hello123", result);
    }

    [Fact]
    public void RemoveUnwantedCharacters_PatternMatchesNothing_ReturnsSameString()
    {
        var result = "Hello".RemoveUnwantedCharacters(@"[0-9]");

        Assert.Equal("Hello", result);
    }

    // ------- RemoveSpecialCharacters -------

    [Fact]
    public void RemoveSpecialCharacters_RemovesSpecialCharacters()
    {
        var result = "Hello!@#$%".RemoveSpecialCharacters();

        Assert.Equal("Hello", result);
    }

    [Fact]
    public void RemoveSpecialCharacters_KeepsAlphanumericHyphenUnderscoreDot()
    {
        var result = "file-name_v1.0".RemoveSpecialCharacters();

        Assert.Equal("file-name_v1.0", result);
    }

    // ------- IsAlphaNumeric -------

    [Fact]
    public void IsAlphaNumeric_AlphaNumericInput_ReturnsTrue()
    {
        Assert.True("Hello123".IsAlphaNumeric());
    }

    [Fact]
    public void IsAlphaNumeric_OnlyLetters_ReturnsTrue()
    {
        Assert.True("HelloWorld".IsAlphaNumeric());
    }

    [Fact]
    public void IsAlphaNumeric_ContainsSpecialChar_ReturnsFalse()
    {
        Assert.False("Hello!".IsAlphaNumeric());
    }

    [Fact]
    public void IsAlphaNumeric_ContainsSpace_ReturnsFalse()
    {
        Assert.False("Hello World".IsAlphaNumeric());
    }

    // ------- ToCurrency -------

    [Fact]
    public void ToCurrency_UsEnglish_FormatsWithDollarSign()
    {
        var result = (1234.56).ToCurrency("en-US");

        Assert.Equal("$1,234.56", result);
    }

    [Fact]
    public void ToCurrency_GbEnglish_FormatsWithPoundSign()
    {
        var result = (99.99).ToCurrency("en-GB");

        Assert.Equal("£99.99", result);
    }

    // ------- Reverse -------

    [Fact]
    public void Reverse_ReversesString()
    {
        Assert.Equal("olleH", "Hello".Reverse());
    }

    [Fact]
    public void Reverse_EmptyString_ReturnsEmptyString()
    {
        Assert.Equal("", "".Reverse());
    }

    [Fact]
    public void Reverse_SingleChar_ReturnsSameChar()
    {
        Assert.Equal("a", "a".Reverse());
    }

    [Fact]
    public void Reverse_Palindrome_ReturnsSameString()
    {
        Assert.Equal("racecar", "racecar".Reverse());
    }

    // ------- IsValidEmail -------

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("user.name+tag@sub.domain.org")]
    [InlineData("  test@example.com  ")] // trimmed before validation
    public void IsValidEmail_ValidEmail_ReturnsTrue(string email)
    {
        Assert.True(email.IsValidEmail());
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing@")]
    [InlineData("@nodomain")]
    [InlineData("plainstring")]
    public void IsValidEmail_InvalidEmail_ReturnsFalse(string email)
    {
        Assert.False(email.IsValidEmail());
    }

    [Fact]
    public void Truncate_LongerThanMax_AppendsEllipsis()
    {
        Assert.Equal("hello…", "hello world".Truncate(5));
    }

    [Fact]
    public void Truncate_ShorterThanMax_ReturnsInput()
    {
        Assert.Equal("hi", "hi".Truncate(10));
    }

    [Fact]
    public void Truncate_CustomEllipsis()
    {
        Assert.Equal("ab...", "abcdef".Truncate(2, "..."));
    }

    [Fact]
    public void Truncate_ZeroMax_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "abc".Truncate(0));
    }

    [Fact]
    public void Truncate_NegativeMax_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Truncate(-1));
    }

    [Fact]
    public void Repeat_Three()
    {
        Assert.Equal("abcabcabc", "abc".Repeat(3));
    }

    [Fact]
    public void Repeat_Zero_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, "abc".Repeat(0));
    }

    [Fact]
    public void Repeat_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Repeat(-1));
    }

    [Fact]
    public void Left_TakesPrefix()
    {
        Assert.Equal("abc", "abcdef".Left(3));
    }

    [Fact]
    public void Left_CountExceedsLength_ReturnsAll()
    {
        Assert.Equal("abc", "abc".Left(100));
    }

    [Fact]
    public void Left_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Left(-1));
    }

    [Fact]
    public void Right_TakesSuffix()
    {
        Assert.Equal("def", "abcdef".Right(3));
    }

    [Fact]
    public void Right_CountExceedsLength_ReturnsAll()
    {
        Assert.Equal("abc", "abc".Right(100));
    }

    [Fact]
    public void Right_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Right(-1));
    }

    [Fact]
    public void RemoveWhitespace_StripsAllSpaces()
    {
        Assert.Equal("helloworld", " hello \t world \n".RemoveWhitespace());
    }

    [Fact]
    public void SplitLines_MixedSeparators()
    {
        var input = "a\r\nb\nc\rd";

        Assert.Equal(["a", "b", "c", "d"], input.SplitLines());
    }

    [Fact]
    public void SplitLines_TrailingNewlineProducesEmptyEntry()
    {
        Assert.Equal(["a", "b", ""], "a\nb\n".SplitLines());
    }

    [Fact]
    public void EnsurePrefix_AddsWhenMissing()
    {
        Assert.Equal("v1.2", "1.2".EnsurePrefix("v"));
    }

    [Fact]
    public void EnsurePrefix_NoOpWhenAlreadyPresent()
    {
        Assert.Equal("v1.2", "v1.2".EnsurePrefix("v"));
    }

    [Fact]
    public void EnsureSuffix_AddsWhenMissing()
    {
        Assert.Equal("path/", "path".EnsureSuffix("/"));
    }

    [Fact]
    public void EnsureSuffix_NoOpWhenAlreadyPresent()
    {
        Assert.Equal("path/", "path/".EnsureSuffix("/"));
    }

    [Theory]
    [InlineData("Hello World", "HELLO", true)]
    [InlineData("Hello World", "world", true)]
    [InlineData("Hello", "xyz", false)]
    public void ContainsIgnoreCase(string source, string other, bool expected)
    {
        Assert.Equal(expected, source.ContainsIgnoreCase(other));
    }

    [Theory]
    [InlineData("Hello", "HELLO", true)]
    [InlineData("Hello", "Hello", true)]
    [InlineData("Hello", "world", false)]
    public void EqualsIgnoreCase(string a, string b, bool expected)
    {
        Assert.Equal(expected, a.EqualsIgnoreCase(b));
    }

    [Theory]
    [InlineData("Hello World", "HELLO", true)]
    [InlineData("Hello World", "world", false)]
    public void StartsWithIgnoreCase(string source, string prefix, bool expected)
    {
        Assert.Equal(expected, source.StartsWithIgnoreCase(prefix));
    }

    [Theory]
    [InlineData("Hello World", "WORLD", true)]
    [InlineData("Hello World", "hello", false)]
    public void EndsWithIgnoreCase(string source, string suffix, bool expected)
    {
        Assert.Equal(expected, source.EndsWithIgnoreCase(suffix));
    }

    [Theory]
    [InlineData("d4a5b8e0-6cf0-4e0e-9b9b-1f2b3c4d5e6f", true)]
    [InlineData("D4A5B8E0-6CF0-4E0E-9B9B-1F2B3C4D5E6F", true)]
    [InlineData("not-a-guid", false)]
    [InlineData("", false)]
    public void IsValidGuid(string value, bool expected)
    {
        Assert.Equal(expected, value.IsValidGuid());
    }

    [Theory]
    [InlineData("https://example.com", true)]
    [InlineData("http://example.com/path?a=1", true)]
    [InlineData("ftp://example.com", false)]
    [InlineData("example.com", false)]
    [InlineData("not a url", false)]
    public void IsValidUrl(string value, bool expected)
    {
        Assert.Equal(expected, value.IsValidUrl());
    }
}
