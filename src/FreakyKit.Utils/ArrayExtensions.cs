namespace FreakyKit.Utils;

public static class ArrayExtensions
{
    /// <summary>
    /// Walks every element of any-rank <paramref name="array"/> and invokes <paramref name="action"/>
    /// with the array and the current N-dimensional index position.
    /// </summary>
    /// <param name="array">The array to traverse. May have any rank; no-op when empty.</param>
    /// <param name="action">Callback receiving the array and the current position (length equals <see cref="Array.Rank"/>).</param>
    public static void ForEach(this Array array, Action<Array, int[]> action)
    {
        if (array.Length == 0) return;
        ArrayTraverse walker = new(array);
        do action(array, walker.Position);
        while (walker.Step());
    }

    /// <summary>
    /// Sets every element of <paramref name="array"/> to <paramref name="value"/>. Wraps <see cref="Array.Fill{T}(T[], T)"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="array">Array to fill (1-D).</param>
    /// <param name="value">Value assigned to every element.</param>
    public static void Fill<T>(this T[] array, T value)
    {
        ArgumentNullException.ThrowIfNull(array);
        Array.Fill(array, value);
    }
}