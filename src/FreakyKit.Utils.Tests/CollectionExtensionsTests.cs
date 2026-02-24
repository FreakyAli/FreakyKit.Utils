namespace FreakyKit.Utils.Tests;

public class CollectionExtensionsTests
{
    [Fact]
    public void AddRange_AddsAllItemsToCollection()
    {
        var list = new List<int> { 1 };

        list.AddRange(2, 3, 4);

        Assert.Equal([1, 2, 3, 4], list);
    }

    [Fact]
    public void AddRange_SubtypeItems_AddsSuccessfully()
    {
        var list = new List<object>();

        list.AddRange<object, string>("hello", "world");

        Assert.Equal(["hello", "world"], list);
    }

    [Fact]
    public void RemoveRange_RemovesSpecifiedItems()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };

        list.RemoveRange(2, 4);

        Assert.Equal([1, 3, 5], list);
    }

    [Fact]
    public void RemoveRange_ItemNotInList_DoesNotThrow()
    {
        var list = new List<int> { 1, 2, 3 };

        list.RemoveRange(99);

        Assert.Equal([1, 2, 3], list);
    }

    [Fact]
    public void RemoveRange_RemovesOnlyFirstOccurrence()
    {
        var list = new List<int> { 1, 2, 2, 3 };

        list.RemoveRange(2);

        Assert.Equal([1, 2, 3], list);
    }
}
