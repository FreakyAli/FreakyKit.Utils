namespace FreakyKit.Utils;

public static class ArrayExtensions
{
    public static void ForEach(this Array array, Action<Array, int[]> action)
    {
        if (array.Length == 0) return;
        ArrayTraverse walker = new(array);
        do action(array, walker.Position);
        while (walker.Step());
    }
}