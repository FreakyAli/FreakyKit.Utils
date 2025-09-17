using System.Xml.Serialization;

namespace FreakyKit.Utils;

public static class ObjectExtensions
{
    public static T? Clone<T>(this T source)
    {
        var serialized = JsonSerializer.Serialize(source);
        var result = JsonSerializer.Deserialize<T>(serialized);
        return result;
    }

    public static bool Is<T>(this object item) where T : class
    {
        return item is T;
    }

    public static bool IsNot<T>(this object item) where T : class
    {
        return !item.Is<T>();
    }

    public static T? As<T>(this object item) where T : class
    {
        return item as T;
    }

    public static string ToJson<T>(this T instance, JsonSerializerOptions? options = null)
        => JsonSerializer.Serialize(instance, options);

    public static T? FromJson<T>(this string json, JsonSerializerOptions? options = null)
        => JsonSerializer.Deserialize<T>(json, options);

    /// <summary>Serializes an object of type T in to an xml string</summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="obj">Object to serialize</param>
    /// <returns>A string that represents Xml, empty otherwise</returns>
    public static string XmlSerialize<T>(this T obj) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(obj);

        var serializer = new XmlSerializer(typeof(T));
        using var writer = new StringWriter();
        serializer.Serialize(writer, obj);
        return writer.ToString();
    }

    /// <summary>Deserializes an xml string in to an object of Type T</summary>
    /// <typeparam name="T">Any class type</typeparam>
    /// <param name="xml">Xml as string to deserialize from</param>
    /// <returns>A new object of type T is successful, null if failed</returns>
    public static T? XmlDeserialize<T>(this string xml) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(xml);

        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xml);
        try { var result = serializer.Deserialize(reader); return (T?)result; }
        catch { return null; } // Could not be deserialized to this type.
    }

    public static bool CompareAsJson(this object obj, object anotherObj)
    {
        if (ReferenceEquals(obj, anotherObj)) return true;
        if ((obj == null) || (anotherObj == null)) return false;
        if (obj.GetType() != anotherObj.GetType()) return false;

        var objJson = JsonSerializer.Serialize(obj);
        var anotherJson = JsonSerializer.Serialize(anotherObj);

        return objJson.Equals(anotherJson, StringComparison.OrdinalIgnoreCase);
    }

}
