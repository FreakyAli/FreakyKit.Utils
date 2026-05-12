using System.Collections.ObjectModel;

namespace FreakyKit.Utils;

public static class DictionaryExtensions
{
    /// <summary>
    /// Returns the existing value for <paramref name="key"/>; if missing, stores <paramref name="value"/>
    /// and returns it.
    /// </summary>
    /// <typeparam name="TKey">Dictionary key type.</typeparam>
    /// <typeparam name="TValue">Dictionary value type.</typeparam>
    /// <param name="dictionary">The dictionary to query or mutate.</param>
    /// <param name="key">Key to look up.</param>
    /// <param name="value">Value stored when the key is absent.</param>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        if (dictionary.TryGetValue(key, out var existing)) return existing;
        dictionary[key] = value;
        return value;
    }

    /// <summary>
    /// Returns the existing value for <paramref name="key"/>; if missing, invokes <paramref name="factory"/>,
    /// stores the result, and returns it.
    /// </summary>
    /// <typeparam name="TKey">Dictionary key type.</typeparam>
    /// <typeparam name="TValue">Dictionary value type.</typeparam>
    /// <param name="dictionary">The dictionary to query or mutate.</param>
    /// <param name="key">Key to look up.</param>
    /// <param name="factory">Value factory invoked when the key is absent.</param>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(factory);
        if (dictionary.TryGetValue(key, out var existing)) return existing;
        var value = factory(key);
        dictionary[key] = value;
        return value;
    }

    /// <summary>
    /// Adds <paramref name="addValue"/> when <paramref name="key"/> is absent, or replaces the existing value
    /// with the result of <paramref name="updateValueFactory"/>(key, oldValue).
    /// </summary>
    /// <typeparam name="TKey">Dictionary key type.</typeparam>
    /// <typeparam name="TValue">Dictionary value type.</typeparam>
    /// <param name="dictionary">The dictionary to mutate.</param>
    /// <param name="key">Key to add or update.</param>
    /// <param name="addValue">Value used when the key is new.</param>
    /// <param name="updateValueFactory">Factory invoked when the key exists.</param>
    /// <returns>The new value stored under <paramref name="key"/>.</returns>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(updateValueFactory);
        if (dictionary.TryGetValue(key, out var existing))
        {
            var updated = updateValueFactory(key, existing);
            dictionary[key] = updated;
            return updated;
        }
        dictionary[key] = addValue;
        return addValue;
    }

    /// <summary>
    /// Copies every entry of <paramref name="other"/> into <paramref name="dictionary"/>, overwriting on
    /// key collision.
    /// </summary>
    /// <typeparam name="TKey">Dictionary key type.</typeparam>
    /// <typeparam name="TValue">Dictionary value type.</typeparam>
    /// <param name="dictionary">Destination dictionary.</param>
    /// <param name="other">Source dictionary to merge in.</param>
    public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> other)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(other);
        foreach (var kvp in other)
            dictionary[kvp.Key] = kvp.Value;
    }

    /// <summary>
    /// Returns a new dictionary with keys and values swapped. Throws when values are not unique.
    /// </summary>
    /// <typeparam name="TKey">Original key type (becomes the value).</typeparam>
    /// <typeparam name="TValue">Original value type (becomes the key); must be non-nullable.</typeparam>
    /// <param name="dictionary">Dictionary to invert.</param>
    /// <exception cref="ArgumentException">Thrown when values are not unique.</exception>
    public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        where TValue : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        var inverted = new Dictionary<TValue, TKey>(dictionary.Count);
        foreach (var kvp in dictionary)
            inverted.Add(kvp.Value, kvp.Key);
        return inverted;
    }

    /// <summary>
    /// Wraps <paramref name="dictionary"/> in a <see cref="ReadOnlyDictionary{TKey, TValue}"/> snapshot.
    /// </summary>
    /// <typeparam name="TKey">Dictionary key type.</typeparam>
    /// <typeparam name="TValue">Dictionary value type.</typeparam>
    /// <param name="dictionary">Dictionary to wrap.</param>
    public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        return new ReadOnlyDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(dictionary));
    }
}
