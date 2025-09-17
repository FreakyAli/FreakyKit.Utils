namespace FreakyKit.Utils;

public static class EnumerableExtensions
{
    public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> col) => [.. col];

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) => self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();

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

    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (var item in sequence)
            action(item);
    }

    public static T SingleOrDefault<T>(this IEnumerable<T> source,
                                    Func<T, bool> action, T theDefault)
    {
        if (source == null)
            return theDefault;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        T item = source.SingleOrDefault(action);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (item != null)
            return item;

        return theDefault;
    }

    public static T FirstOrDefault<T>(this IEnumerable<T> source,
                                    Func<T, bool> action, T theDefault)
    {
        if (source == null)
            return theDefault;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        T item = source.FirstOrDefault(action);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (item != null)
            return item;

        return theDefault;
    }

    public static T ElementAtOrDefault<T>(this IEnumerable<T> source,
                                    int index, T theDefault)
    {
        if (source == null)
            return theDefault;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        T item = source.ElementAtOrDefault(index);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (item != null)
            return item;

        return theDefault;
    }

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
    {
        return source ?? [];
    }

    public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource element)
    {
        using IEnumerator<TSource> e1 = source.GetEnumerator();
        while (e1.MoveNext())
            yield return e1.Current;

        yield return element;
    }

    public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource element)
    {
        yield return element;

        using IEnumerator<TSource> e1 = source.GetEnumerator();
        while (e1.MoveNext())
            yield return e1.Current;
    }

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
}