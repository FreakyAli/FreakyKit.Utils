namespace FreakyKit.Utils.Tests;

public class ObjectExtensionsTests
{
    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    // ------- Clone -------

    [Fact]
    public void Clone_ReturnsDeepCopyWithSameValues()
    {
        var original = new Person { Name = "Alice", Age = 30 };

        var clone = original.Clone();

        Assert.NotNull(clone);
        Assert.Equal("Alice", clone!.Name);
        Assert.Equal(30, clone.Age);
    }

    [Fact]
    public void Clone_ModifyingCloneDoesNotAffectOriginal()
    {
        var original = new Person { Name = "Alice", Age = 30 };

        var clone = original.Clone()!;
        clone.Name = "Bob";

        Assert.Equal("Alice", original.Name);
    }

    // ------- Is / IsNot / As -------

    [Fact]
    public void Is_CorrectType_ReturnsTrue()
    {
        object item = "hello";

        Assert.True(item.Is<string>());
    }

    [Fact]
    public void Is_IncorrectType_ReturnsFalse()
    {
        object item = 42;

        Assert.False(item.Is<string>());
    }

    [Fact]
    public void IsNot_CorrectType_ReturnsFalse()
    {
        object item = "hello";

        Assert.False(item.IsNot<string>());
    }

    [Fact]
    public void IsNot_IncorrectType_ReturnsTrue()
    {
        object item = 42;

        Assert.True(item.IsNot<string>());
    }

    [Fact]
    public void As_CorrectType_ReturnsTypedObject()
    {
        object item = "hello";

        var result = item.As<string>();

        Assert.Equal("hello", result);
    }

    [Fact]
    public void As_IncorrectType_ReturnsNull()
    {
        object item = 42;

        var result = item.As<string>();

        Assert.Null(result);
    }

    // ------- ToJson / FromJson -------

    [Fact]
    public void ToJson_SerializesObjectToJsonString()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        var json = person.ToJson();

        Assert.Contains("Alice", json);
        Assert.Contains("30", json);
    }

    [Fact]
    public void FromJson_DeserializesJsonStringToObject()
    {
        var json = """{"Name":"Alice","Age":30}""";

        var result = json.FromJson<Person>();

        Assert.NotNull(result);
        Assert.Equal("Alice", result!.Name);
        Assert.Equal(30, result.Age);
    }

    [Fact]
    public void ToJson_FromJson_Roundtrip()
    {
        var original = new Person { Name = "Bob", Age = 25 };

        var result = original.ToJson().FromJson<Person>();

        Assert.NotNull(result);
        Assert.Equal(original.Name, result!.Name);
        Assert.Equal(original.Age, result.Age);
    }

    // ------- XmlSerialize / XmlDeserialize -------

    [Fact]
    public void XmlSerialize_SerializesObjectToXmlString()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        var xml = person.XmlSerialize();

        Assert.Contains("Alice", xml);
        Assert.Contains("30", xml);
    }

    [Fact]
    public void XmlDeserialize_DeserializesXmlStringToObject()
    {
        var person = new Person { Name = "Bob", Age = 25 };
        var xml = person.XmlSerialize();

        var result = xml.XmlDeserialize<Person>();

        Assert.NotNull(result);
        Assert.Equal("Bob", result!.Name);
        Assert.Equal(25, result.Age);
    }

    [Fact]
    public void XmlDeserialize_InvalidXml_ReturnsNull()
    {
        var xml = "this is not valid xml at all";

        var result = xml.XmlDeserialize<Person>();

        Assert.Null(result);
    }

    [Fact]
    public void XmlSerialize_NullObject_ThrowsArgumentNullException()
    {
        Person person = null!;

        Assert.Throws<ArgumentNullException>(() => person.XmlSerialize());
    }

    // ------- CompareAsJson -------

    [Fact]
    public void CompareAsJson_ObjectsWithSameValues_ReturnsTrue()
    {
        var obj1 = new Person { Name = "Alice", Age = 30 };
        var obj2 = new Person { Name = "Alice", Age = 30 };

        Assert.True(obj1.CompareAsJson(obj2));
    }

    [Fact]
    public void CompareAsJson_ObjectsWithDifferentValues_ReturnsFalse()
    {
        var obj1 = new Person { Name = "Alice", Age = 30 };
        var obj2 = new Person { Name = "Bob", Age = 25 };

        Assert.False(obj1.CompareAsJson(obj2));
    }

    [Fact]
    public void CompareAsJson_SameReference_ReturnsTrue()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        Assert.True(person.CompareAsJson(person));
    }

    [Fact]
    public void CompareAsJson_DifferentTypes_ReturnsFalse()
    {
        object obj1 = new Person { Name = "Alice", Age = 30 };
        object obj2 = "Alice";

        Assert.False(obj1.CompareAsJson(obj2));
    }

    [Fact]
    public void CompareAsJson_NullSecondObject_ReturnsFalse()
    {
        var person = new Person { Name = "Alice", Age = 30 };

        Assert.False(person.CompareAsJson(null!));
    }
}
