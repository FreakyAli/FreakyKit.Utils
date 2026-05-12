namespace FreakyKit.Utils.Tests;

public class TypeExtensionsTests
{
    [AttributeUsage(AttributeTargets.Class)]
    private sealed class MarkerAttribute : Attribute { }

    [Marker]
    private sealed class MarkedType { }

    private sealed class UnmarkedType { }

    private abstract class AbstractBase { }
    private interface IThing { }

    [Fact]
    public void IsAssignableTo_DerivedToBase_True()
    {
        Assert.True(typeof(string).IsAssignableTo<object>());
    }

    [Fact]
    public void IsAssignableTo_UnrelatedType_False()
    {
        Assert.False(typeof(int).IsAssignableTo<string>());
    }

    [Fact]
    public void GetGenericTypeName_NonGeneric_ReturnsName()
    {
        Assert.Equal("Int32", typeof(int).GetGenericTypeName());
    }

    [Fact]
    public void GetGenericTypeName_Generic_ReadableForm()
    {
        Assert.Equal("List<Int32>", typeof(List<int>).GetGenericTypeName());
    }

    [Fact]
    public void GetGenericTypeName_NestedGeneric()
    {
        Assert.Equal("Dictionary<String, List<Int32>>", typeof(Dictionary<string, List<int>>).GetGenericTypeName());
    }

    [Fact]
    public void HasAttribute_True()
    {
        Assert.True(typeof(MarkedType).HasAttribute<MarkerAttribute>());
    }

    [Fact]
    public void HasAttribute_False()
    {
        Assert.False(typeof(UnmarkedType).HasAttribute<MarkerAttribute>());
    }

    [Fact]
    public void GetAttribute_ReturnsInstance()
    {
        Assert.NotNull(typeof(MarkedType).GetAttribute<MarkerAttribute>());
    }

    [Fact]
    public void GetAttribute_Missing_ReturnsNull()
    {
        Assert.Null(typeof(UnmarkedType).GetAttribute<MarkerAttribute>());
    }

    [Fact]
    public void IsNullable_NullableInt_True()
    {
        Assert.True(typeof(int?).IsNullable());
    }

    [Fact]
    public void IsNullable_Int_False()
    {
        Assert.False(typeof(int).IsNullable());
    }

    [Fact]
    public void IsNullable_ReferenceType_False()
    {
        // Open question: T? for class is the same Type as T at runtime. IsNullable specifically targets Nullable<T>.
        Assert.False(typeof(string).IsNullable());
    }

    [Fact]
    public void GetDefaultValue_ValueType_ReturnsZero()
    {
        Assert.Equal(0, typeof(int).GetDefaultValue());
    }

    [Fact]
    public void GetDefaultValue_ReferenceType_ReturnsNull()
    {
        Assert.Null(typeof(string).GetDefaultValue());
    }

    [Fact]
    public void IsConcrete_NormalClass_True()
    {
        Assert.True(typeof(UnmarkedType).IsConcrete());
    }

    [Fact]
    public void IsConcrete_Abstract_False()
    {
        Assert.False(typeof(AbstractBase).IsConcrete());
    }

    [Fact]
    public void IsConcrete_Interface_False()
    {
        Assert.False(typeof(IThing).IsConcrete());
    }

    [Fact]
    public void IsConcrete_OpenGeneric_False()
    {
        Assert.False(typeof(List<>).IsConcrete());
    }
}
