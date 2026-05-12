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

        ((ICollection<int>)list).RemoveRange(2, 4);

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

    [Fact]
    public void RemoveWhere_List_RemovesMatching()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };

        var removed = list.RemoveWhere(x => x % 2 == 0);

        Assert.Equal(2, removed);
        Assert.Equal([1, 3, 5], list);
    }

    [Fact]
    public void RemoveWhere_HashSet_RemovesMatching()
    {
        ICollection<int> set = new HashSet<int> { 1, 2, 3, 4 };

        var removed = set.RemoveWhere(x => x > 2);

        Assert.Equal(2, removed);
        Assert.Equal([1, 2], set.Order().ToList());
    }

    [Fact]
    public void RemoveWhere_NoMatch_ReturnsZero()
    {
        var list = new List<int> { 1, 2, 3 };

        Assert.Equal(0, list.RemoveWhere(x => x > 100));
        Assert.Equal([1, 2, 3], list);
    }

    [Fact]
    public void Replace_List_ReplacesAtPosition()
    {
        var list = new List<int> { 1, 2, 3 };

        Assert.True(list.Replace(2, 20));
        Assert.Equal([1, 20, 3], list);
    }

    [Fact]
    public void Replace_HashSet_RemovesAndAdds()
    {
        ICollection<int> set = new HashSet<int> { 1, 2, 3 };

        Assert.True(set.Replace(2, 20));
        Assert.DoesNotContain(2, set);
        Assert.Contains(20, set);
    }

    [Fact]
    public void Replace_ItemMissing_ReturnsFalse()
    {
        var list = new List<int> { 1, 2 };

        Assert.False(list.Replace(99, 100));
        Assert.Equal([1, 2], list);
    }
}
