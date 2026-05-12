using System;

namespace FreakyKit.Utils;

public static class CollectionExtensions
{
    /// <summary>
    /// Appends every item in <paramref name="values"/> to <paramref name="list"/> in order.
    /// </summary>
    /// <typeparam name="T">Element type of the collection.</typeparam>
    /// <typeparam name="S">Type of the items being added; must derive from <typeparamref name="T"/>.</typeparam>
    /// <param name="list">The collection to add to.</param>
    /// <param name="values">Items to append.</param>
    public static void AddRange<T, S>(this ICollection<T> list, params S[] values)
    where S : T
    {
        foreach (S value in values)
            list.Add(value);
    }

    /// <summary>
    /// Calls <see cref="ICollection{T}.Remove"/> for every item in <paramref name="values"/>.
    /// Items not present in the collection are silently ignored.
    /// </summary>
    /// <typeparam name="T">Element type of the collection.</typeparam>
    /// <typeparam name="S">Type of the items being removed; must derive from <typeparamref name="T"/>.</typeparam>
    /// <param name="list">The collection to remove from.</param>
    /// <param name="values">Items to remove.</param>
    public static void RemoveRange<T, S>(this ICollection<T> list, params S[] values)
    where S : T
    {
        foreach (S value in values)
            list.Remove(value);
    }

    /// <summary>
    /// Removes every element of <paramref name="collection"/> matching <paramref name="predicate"/>.
    /// Delegates to <see cref="List{T}.RemoveAll(Predicate{T})"/> when possible.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="collection">The collection to mutate.</param>
    /// <param name="predicate">Predicate identifying elements to remove.</param>
    /// <returns>The number of elements removed.</returns>
    public static int RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        if (collection is List<T> list)
            return list.RemoveAll(new Predicate<T>(predicate));

        var toRemove = collection.Where(predicate).ToList();
        foreach (var item in toRemove)
            collection.Remove(item);
        return toRemove.Count;
    }

    /// <summary>
    /// Replaces the first occurrence of <paramref name="oldItem"/> with <paramref name="newItem"/>.
    /// For <see cref="IList{T}"/> the replacement preserves the original position; otherwise the item is removed and re-added.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="collection">The collection to mutate.</param>
    /// <param name="oldItem">Item to find.</param>
    /// <param name="newItem">Replacement item.</param>
    /// <returns><c>true</c> when <paramref name="oldItem"/> was found and replaced; <c>false</c> otherwise.</returns>
    public static bool Replace<T>(this ICollection<T> collection, T oldItem, T newItem)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection is IList<T> list)
        {
            int index = list.IndexOf(oldItem);
            if (index < 0) return false;
            list[index] = newItem;
            return true;
        }

        if (!collection.Remove(oldItem)) return false;
        collection.Add(newItem);
        return true;
    }
}