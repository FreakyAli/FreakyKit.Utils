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
}
