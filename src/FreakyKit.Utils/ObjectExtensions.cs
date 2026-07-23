using System.Xml.Serialization;

namespace FreakyKit.Utils;

public static class ObjectExtensions
{
    /// <summary>
    /// Produces a deep copy of <paramref name="source"/> by round-tripping it through <see cref="JsonSerializer"/>.
    /// </summary>
    /// <remarks>
    /// Only state visible to <see cref="System.Text.Json"/> is copied: public, JSON-serializable properties.
    /// Private fields, non-serializable members, reference identity, and cycles are not preserved.
    /// </remarks>
    public static T? Clone<T>(this T source)
    {
        var serialized = JsonSerializer.Serialize(source);
        var result = JsonSerializer.Deserialize<T>(serialized);
        return result;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="item"/> is an instance of <typeparamref name="T"/>.
    /// Sugar for the <c>is</c> operator.
    /// </summary>
    /// <typeparam name="T">Reference type to test against.</typeparam>
    /// <param name="item">The object to test.</param>
    public static bool Is<T>(this object item) where T : class
    {
        return item is T;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="item"/> is <em>not</em> an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Reference type to test against.</typeparam>
    /// <param name="item">The object to test.</param>
    public static bool IsNot<T>(this object item) where T : class
    {
        return !item.Is<T>();
    }

    /// <summary>
    /// Returns <paramref name="item"/> cast to <typeparamref name="T"/>, or <c>null</c> if the cast fails.
    /// Sugar for the <c>as</c> operator.
    /// </summary>
    /// <typeparam name="T">Reference type to cast to.</typeparam>
    /// <param name="item">The object to cast.</param>
    public static T? As<T>(this object item) where T : class
    {
        return item as T;
    }

    /// <summary>
    /// Serializes <paramref name="instance"/> to a JSON string using <see cref="JsonSerializer"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value being serialized.</typeparam>
    /// <param name="instance">The value to serialize.</param>
    /// <param name="options">Optional serializer options.</param>
    public static string ToJson<T>(this T instance, JsonSerializerOptions? options = null)
        => JsonSerializer.Serialize(instance, options);

    /// <summary>
    /// Deserializes <paramref name="json"/> to an instance of <typeparamref name="T"/> using <see cref="JsonSerializer"/>.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    /// <param name="json">JSON document to deserialize.</param>
    /// <param name="options">Optional serializer options.</param>
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

    /// <summary>
    /// Performs a structural equality check by serializing both objects to JSON and comparing the result
    /// case-insensitively. Returns <c>true</c> when both serialize to the same payload.
    /// </summary>
    /// <remarks>
    /// Only state visible to <see cref="System.Text.Json"/> participates. Different runtime types compare unequal.
    /// </remarks>
    /// <param name="obj">First operand.</param>
    /// <param name="anotherObj">Second operand.</param>
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
