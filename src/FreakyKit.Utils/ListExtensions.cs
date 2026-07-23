namespace FreakyKit.Utils;

public static class ListExtensions
{
    /// <summary>
    /// Removes all items from the provided <paramref name="list"/> that match the<paramref name="predicate"/> expression.
    /// </summary>
    /// <typeparam name="T">The class type of the list items.</typeparam>
    /// <param name="list">The list to remove items from.</param>
    /// <param name="predicate">The predicate expression to test against.</param>
    public static void RemoveAll<T>(this IList<T> instance, Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(predicate);
        if (instance is T[])
            throw new NotSupportedException();

        if (instance is List<T> list)
        {
            list.RemoveAll(predicate);
            return;
        }

        int writeIndex = 0;
        for (int readIndex = 0; readIndex < instance.Count; readIndex++)
        {
            var item = instance[readIndex];
            if (predicate(item)) continue;

            if (readIndex != writeIndex)
            {
                instance[writeIndex] = item;
            }
            ++writeIndex;
        }

        if (writeIndex != instance.Count)
        {
            for (int deleteIndex = instance.Count - 1; deleteIndex >= writeIndex; --deleteIndex)
            {
                instance.RemoveAt(deleteIndex);
            }
        }
    }

    /// <summary>
    /// Inserts an Item into a list at the first place that the <paramref name="predicate"/> expression fails.  If it is true in all cases, then the item is appended to the end of the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="obj"></param>
    /// <param name="predicate">The sepcified function that determines when the <paramref name="obj"/> should be added. </param>
    public static void InsertWhere<T>(this IList<T> list, T obj, Func<T, bool> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            // When the function first fails it inserts the obj paramiter. 
            // For example, in a list myList of ordered Int32's {1,2,3,4,5,10,12}
            // Calling myList.InsertWhere( 8, x => 8 > x) inserts 8 once the list item becomes greater then or equal to it.
            if (!predicate(list[i]))
            {
                list.Insert(i, obj);
                return;
            }
        }

        list.Add(obj);
    }

    /// <summary>
    /// Performs a binary search on <paramref name="list"/> using a projected key. The list must be sorted by
    /// the same projection.
    /// </summary>
    /// <typeparam name="T">Element type of the list.</typeparam>
    /// <typeparam name="TKey">Type of the comparison key.</typeparam>
    /// <param name="list">The sorted list to search.</param>
    /// <param name="keySelector">Function that extracts the comparison key from each element.</param>
    /// <param name="key">The key value to find.</param>
    /// <returns>The element whose projected key equals <paramref name="key"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no matching element is found.</exception>
    public static T BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key)
        where TKey : IComparable<TKey>
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(keySelector);

        int min = 0;
        int max = list.Count;
        while (min < max)
        {
            int mid = min + ((max - min) / 2);
            TKey midKey = keySelector(list[mid]);
            int comp = Comparer<TKey>.Default.Compare(midKey, key);
            if (comp < 0)
            {
                min = mid + 1;
            }
            else if (comp > 0)
            {
                max = mid;
            }
            else
            {
                return list[mid];
            }
        }
        throw new InvalidOperationException("Item not found");
    }

    /// <summary>
    /// Swaps the elements at <paramref name="i"/> and <paramref name="j"/> in place.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="list">The list to mutate.</param>
    /// <param name="i">Index of the first element.</param>
    /// <param name="j">Index of the second element.</param>
    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        ArgumentNullException.ThrowIfNull(list);
        if (i < 0 || i >= list.Count) throw new ArgumentOutOfRangeException(nameof(i));
        if (j < 0 || j >= list.Count) throw new ArgumentOutOfRangeException(nameof(j));
        if (i == j) return;
        (list[i], list[j]) = (list[j], list[i]);
    }

    /// <summary>
    /// Removes the element at <paramref name="fromIndex"/> and re-inserts it at <paramref name="toIndex"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="list">The list to mutate.</param>
    /// <param name="fromIndex">Source index.</param>
    /// <param name="toIndex">Destination index after removal.</param>
    public static void Move<T>(this IList<T> list, int fromIndex, int toIndex)
    {
        ArgumentNullException.ThrowIfNull(list);
        if (fromIndex < 0 || fromIndex >= list.Count) throw new ArgumentOutOfRangeException(nameof(fromIndex));
        if (toIndex < 0 || toIndex >= list.Count) throw new ArgumentOutOfRangeException(nameof(toIndex));
        if (fromIndex == toIndex) return;
        var item = list[fromIndex];
        list.RemoveAt(fromIndex);
        list.Insert(toIndex, item);
    }
}
