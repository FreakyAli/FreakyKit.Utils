namespace FreakyKit.Utils.Tests;

public class ArrayExtensionsTests
{
    [Fact]
    public void ForEach_1DArray_VisitsAllElementsInOrder()
    {
        int[] array = [1, 2, 3, 4, 5];
        var visited = new List<int>();

        array.ForEach((arr, idx) => visited.Add((int)arr.GetValue(idx[0])!));

        Assert.Equal([1, 2, 3, 4, 5], visited);
    }

    [Fact]
    public void ForEach_2DArray_VisitsAllElements()
    {
        int[,] array = { { 1, 2 }, { 3, 4 } };
        var visited = new List<int>();

        array.ForEach((arr, idx) => visited.Add((int)arr.GetValue(idx[0], idx[1])!));

        Assert.Equal(4, visited.Count);
        Assert.Contains(1, visited);
        Assert.Contains(2, visited);
        Assert.Contains(3, visited);
        Assert.Contains(4, visited);
    }

    [Fact]
    public void ForEach_EmptyArray_DoesNotInvokeAction()
    {
        int[] array = [];
        int callCount = 0;

        array.ForEach((_, _) => callCount++);

        Assert.Equal(0, callCount);
    }
}
