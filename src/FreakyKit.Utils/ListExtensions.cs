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
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));
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

    public static T BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key)
        where TKey : IComparable<TKey>
    {
        int min = 0;
        int max = list.Count;
        while (min < max)
        {
            int mid = (max + min) / 2;
            T midItem = list[mid];
            TKey midKey = keySelector(midItem);
            int comp = midKey.CompareTo(key);
            if (comp < 0)
            {
                min = mid + 1;
            }
            else if (comp > 0)
            {
                max = mid - 1;
            }
            else
            {
                return midItem;
            }
        }
        if (min == max &&
            keySelector(list[min]).CompareTo(key) == 0)
        {
            return list[min];
        }
        throw new InvalidOperationException("Item not found");
    }
}
