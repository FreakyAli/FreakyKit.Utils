using System;

namespace FreakyKit.Utils;

public static class CollectionExtensions
{
    public static void AddRange<T, S>(this ICollection<T> list, params S[] values)
    where S : T
    {
        foreach (S value in values)
            list.Add(value);
    }

    public static void RemoveRange<T, S>(this ICollection<T> list, params S[] values)
    where S : T
    {
        foreach (S value in values)
            list.Remove(value);
    }
}