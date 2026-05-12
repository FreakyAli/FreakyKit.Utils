namespace FreakyKit.Utils;

public static class EnumerableExtensions
{
    /// <summary>
    /// Materializes <paramref name="col"/> into a new <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="col">Source sequence.</param>
    public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> col) => [.. col];

    /// <summary>
    /// Pairs every element with its zero-based index. Returns an empty sequence when <paramref name="self"/> is <c>null</c>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="self">Source sequence.</param>
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) => self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();

    /// <summary>
    /// Returns <c>true</c> when <paramref name="enumerable"/> is <c>null</c> or contains no elements.
    /// Uses <see cref="ICollection{T}.Count"/> when available to avoid enumerating.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="enumerable">Sequence to test.</param>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            return true;
        }
        if (enumerable is ICollection<T> collection)
        {
            return collection.Count < 1;
        }
        return !enumerable.Any();
    }

    /// <summary>
    /// Yields elements with distinct keys, where the key is produced by <paramref name="keySelector"/>.
    /// Preserves source ordering and keeps the first occurrence of each key.
    /// </summary>
    /// <typeparam name="TSource">Element type.</typeparam>
    /// <typeparam name="TKey">Type of the projected key.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="keySelector">Function projecting each element to its uniqueness key.</param>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = [];
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Invokes <paramref name="action"/> for every element of <paramref name="sequence"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="sequence">Source sequence.</param>
    /// <param name="action">Callback invoked once per element.</param>
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (var item in sequence)
            action(item);
    }

    /// <summary>
    /// Null-safe <see cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool}, TSource)"/>
    /// — returns <paramref name="theDefault"/> when <paramref name="source"/> is <c>null</c>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence, may be <c>null</c>.</param>
    /// <param name="action">Predicate used to locate the element.</param>
    /// <param name="theDefault">Value returned when no element matches or the source is <c>null</c>.</param>
    public static T SingleOrDefault<T>(this IEnumerable<T> source,
                                    Func<T, bool> action, T theDefault)
    {
        if (source == null)
            return theDefault;
        return Enumerable.SingleOrDefault(source, action, theDefault);
    }

    /// <summary>
    /// Null-safe <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool}, TSource)"/>
    /// — returns <paramref name="theDefault"/> when <paramref name="source"/> is <c>null</c>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence, may be <c>null</c>.</param>
    /// <param name="action">Predicate used to locate the element.</param>
    /// <param name="theDefault">Value returned when no element matches or the source is <c>null</c>.</param>
    public static T FirstOrDefault<T>(this IEnumerable<T> source,
                                    Func<T, bool> action, T theDefault)
    {
        if (source == null)
            return theDefault;
        return Enumerable.FirstOrDefault(source, action, theDefault);
    }

    /// <summary>
    /// Null- and range-safe variant of <see cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
    /// — returns <paramref name="theDefault"/> when the source is <c>null</c> or <paramref name="index"/> is out of range.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence, may be <c>null</c>.</param>
    /// <param name="index">Zero-based index of the element to return.</param>
    /// <param name="theDefault">Value returned when the index is invalid or the source is <c>null</c>.</param>
    public static T ElementAtOrDefault<T>(this IEnumerable<T> source,
                                    int index, T theDefault)
    {
        if (source == null || index < 0)
            return theDefault;
        int current = 0;
        foreach (var item in source)
        {
            if (current++ == index)
                return item;
        }
        return theDefault;
    }

    /// <summary>
    /// Returns <paramref name="source"/> when non-null; otherwise an empty sequence.
    /// Useful to fold null-checks into a <c>foreach</c> or LINQ chain.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence, may be <c>null</c>.</param>
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
    {
        return source ?? [];
    }

    /// <summary>
    /// Returns the elements of <paramref name="source"/> in a uniformly random order using Fisher–Yates.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence; must not be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
    static public IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return ShuffleIterator(source);
    }

    static private IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source)
    {
        T[] array = [.. source];
        Random rnd = new();
        for (int n = array.Length; n > 1;)
        {
            int k = rnd.Next(n--); // 0 <= k < n

            //Swap items
            if (n != k)
            {
                (array[n], array[k]) = (array[k], array[n]);
            }
        }

        foreach (var item in array) yield return item;
    }

    /// <summary>
    /// Filters out <c>null</c> elements from a sequence of reference types.
    /// </summary>
    /// <typeparam name="T">Reference element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
    {
        ArgumentNullException.ThrowIfNull(source);
        foreach (var item in source)
            if (item is not null) yield return item;
    }

    /// <summary>
    /// Filters out <c>null</c> elements from a sequence of <see cref="Nullable{T}"/> values,
    /// returning a sequence of the underlying values.
    /// </summary>
    /// <typeparam name="T">Underlying value type.</typeparam>
    /// <param name="source">Source sequence.</param>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : struct
    {
        ArgumentNullException.ThrowIfNull(source);
        foreach (var item in source)
            if (item.HasValue) yield return item.Value;
    }

    /// <summary>
    /// Sugar for <see cref="string.Join{T}(string, IEnumerable{T})"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="separator">String inserted between elements.</param>
    public static string JoinString<T>(this IEnumerable<T> source, string separator) =>
        string.Join(separator, source);

    /// <summary>
    /// Returns the zero-based index of the first element matching <paramref name="predicate"/>, or <c>-1</c> if none match.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="predicate">Predicate used to locate the element.</param>
    public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);
        int i = 0;
        foreach (var item in source)
        {
            if (predicate(item)) return i;
            i++;
        }
        return -1;
    }

    /// <summary>
    /// Returns <c>true</c> when no element of <paramref name="source"/> matches <paramref name="predicate"/>.
    /// Inverse of <see cref="Enumerable.Any{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="predicate">Predicate used to test elements.</param>
    public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate) => !source.Any(predicate);

    /// <summary>
    /// Splits <paramref name="source"/> into the elements that satisfy <paramref name="predicate"/> and those that don't.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="predicate">Predicate used to split elements.</param>
    /// <returns>A tuple <c>(matched, unmatched)</c>.</returns>
    public static (IReadOnlyList<T> matched, IReadOnlyList<T> unmatched) Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);
        var matched = new List<T>();
        var unmatched = new List<T>();
        foreach (var item in source)
        {
            if (predicate(item)) matched.Add(item);
            else unmatched.Add(item);
        }
        return (matched, unmatched);
    }

    /// <summary>
    /// Returns up to <paramref name="count"/> elements chosen uniformly at random from <paramref name="source"/>,
    /// using a Fisher–Yates partial shuffle. If <paramref name="count"/> is larger than the source size,
    /// every element is returned (in random order).
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Number of elements to sample (must be non-negative).</param>
    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (count == 0) return [];
        return source.Shuffle().Take(count);
    }
}