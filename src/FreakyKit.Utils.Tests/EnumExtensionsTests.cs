using System.ComponentModel;

namespace FreakyKit.Utils.Tests;

public class EnumExtensionsTests
{
    private enum Status
    {
        [Description("On the way")]
        InTransit,
        Delivered,
        [Description("Returned to sender")]
        Returned
    }

    [Fact]
    public void GetDescription_WithAttribute_ReturnsDescription()
    {
        Assert.Equal("On the way", Status.InTransit.GetDescription());
    }

    [Fact]
    public void GetDescription_WithoutAttribute_ReturnsName()
    {
        Assert.Equal("Delivered", Status.Delivered.GetDescription());
    }

    [Fact]
    public void IsDefined_DefinedValue_True()
    {
        Assert.True(Status.Delivered.IsDefined());
    }

    [Fact]
    public void IsDefined_UndefinedValue_False()
    {
        Assert.False(((Status)99).IsDefined());
    }

    [Fact]
    public void ToEnum_CaseInsensitive()
    {
        Assert.Equal(Status.Delivered, "delivered".ToEnum<Status>());
    }

    [Fact]
    public void ToEnum_Invalid_Throws()
    {
        Assert.Throws<ArgumentException>(() => "nope".ToEnum<Status>());
    }

    [Fact]
    public void TryToEnum_Valid_True()
    {
        Assert.True("Returned".TryToEnum<Status>(out var result));
        Assert.Equal(Status.Returned, result);
    }

    [Fact]
    public void TryToEnum_Invalid_False()
    {
        Assert.False("garbage".TryToEnum<Status>(out var result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void TryToEnum_NullOrEmpty_False()
    {
        Assert.False(((string)null!).TryToEnum<Status>(out _));
        Assert.False("".TryToEnum<Status>(out _));
    }

    [Fact]
    public void ToEnum_UndefinedNumericValue_Throws()
    {
        // Status enum has members 0, 1, 2; numeric string "99" is undefined
        Assert.Throws<ArgumentException>(() => "99".ToEnum<Status>());
    }

    [Fact]
    public void TryToEnum_UndefinedNumericValue_False()
    {
        // Numeric string that doesn't map to a defined enum member
        Assert.False("99".TryToEnum<Status>(out var result));
        Assert.Equal(default, result);
    }
}
