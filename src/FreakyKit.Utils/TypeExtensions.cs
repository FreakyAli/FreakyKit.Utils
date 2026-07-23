using System.Reflection;

namespace FreakyKit.Utils;

public static class TypeExtensions
{
    /// <summary>
    /// Returns <c>true</c> when <paramref name="type"/> can be assigned to a variable of type <typeparamref name="T"/>.
    /// Inverse direction of <see cref="Type.IsAssignableFrom"/>.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    /// <param name="type">The source type.</param>
    public static bool IsAssignableTo<T>(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return typeof(T).IsAssignableFrom(type);
    }

    /// <summary>
    /// Returns a human-readable name for a generic type — e.g. <c>List&lt;Int32&gt;</c> instead of
    /// <c>List`1[Int32]</c>. Non-generic types return <see cref="MemberInfo.Name"/>.
    /// </summary>
    /// <param name="type">Type to format.</param>
    public static string GetGenericTypeName(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsGenericType) return type.Name;
        var name = type.Name;
        var tick = name.IndexOf('`');
        if (tick >= 0) name = name[..tick];
        var args = string.Join(", ", type.GetGenericArguments().Select(GetGenericTypeName));
        return $"{name}<{args}>";
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="type"/> is decorated with <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type to look for.</typeparam>
    /// <param name="type">Type to inspect.</param>
    /// <param name="inherit">Whether to inspect ancestor types as well.</param>
    public static bool HasAttribute<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetCustomAttribute<TAttribute>(inherit) is not null;
    }

    /// <summary>
    /// Returns the first <typeparamref name="TAttribute"/> on <paramref name="type"/>, or <c>null</c> if absent.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type to look for.</typeparam>
    /// <param name="type">Type to inspect.</param>
    /// <param name="inherit">Whether to inspect ancestor types as well.</param>
    public static TAttribute? GetAttribute<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetCustomAttribute<TAttribute>(inherit);
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="type"/> is <see cref="Nullable{T}"/>.
    /// </summary>
    /// <param name="type">Type to test.</param>
    public static bool IsNullable(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return Nullable.GetUnderlyingType(type) is not null;
    }

    /// <summary>
    /// Returns the language-level default for <paramref name="type"/> — <c>null</c> for reference types,
    /// zero/empty for value types.
    /// </summary>
    /// <param name="type">Type to default.</param>
    public static object? GetDefaultValue(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="type"/> is a non-abstract, non-interface, non-generic-definition
    /// type that can be instantiated (subject to a constructor existing).
    /// </summary>
    /// <param name="type">Type to test.</param>
    public static bool IsConcrete(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return !type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition;
    }
}
