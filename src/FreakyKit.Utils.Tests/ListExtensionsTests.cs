namespace FreakyKit.Utils.Tests;

public class ListExtensionsTests
{
    // A minimal IList<T> that is neither List<T> nor T[], used to exercise the fallback path.
    private sealed class CustomList<T> : IList<T>
    {
        private readonly List<T> _inner = [];

        public CustomList(IEnumerable<T> items) => _inner.AddRange(items);

        public T this[int index] { get => _inner[index]; set => _inner[index] = value; }
        public int Count => _inner.Count;
        public bool IsReadOnly => false;
        public void Add(T item) => _inner.Add(item);
        public void Clear() => _inner.Clear();
        public bool Contains(T item) => _inner.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _inner.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();
        public int IndexOf(T item) => _inner.IndexOf(item);
        public void Insert(int index, T item) => _inner.Insert(index, item);
        public bool Remove(T item) => _inner.Remove(item);
        public void RemoveAt(int index) => _inner.RemoveAt(index);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // ------- RemoveAll -------

    [Fact]
    public void RemoveAll_OnList_RemovesMatchingItems()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };

        list.RemoveAll(x => x % 2 == 0);

        Assert.Equal([1, 3, 5], list);
    }

    [Fact]
    public void RemoveAll_OnCustomIList_RemovesMatchingItems()
    {
        IList<int> list = new CustomList<int>([1, 2, 3, 4, 5]);

        list.RemoveAll(x => x % 2 == 0);

        Assert.Equal([1, 3, 5], list);
    }

    [Fact]
    public void RemoveAll_OnArray_ThrowsNotSupportedException()
    {
        IList<int> array = new[] { 1, 2, 3 };

        Assert.Throws<NotSupportedException>(() => array.RemoveAll(x => x > 1));
    }

    [Fact]
    public void RemoveAll_NullInstance_ThrowsArgumentNullException()
    {
        IList<int> list = null!;

        Assert.Throws<ArgumentNullException>(() => list.RemoveAll(x => true));
    }

    [Fact]
    public void RemoveAll_NullPredicate_ThrowsArgumentNullException()
    {
        IList<int> list = new List<int> { 1, 2 };

        Assert.Throws<ArgumentNullException>(() => list.RemoveAll(null!));
    }

    [Fact]
    public void RemoveAll_NoMatch_LeavesListUnchanged()
    {
        var list = new List<int> { 1, 2, 3 };

        list.RemoveAll(x => x > 100);

        Assert.Equal([1, 2, 3], list);
    }

    // ------- InsertWhere -------

    [Fact]
    public void InsertWhere_InsertsAtFirstPositionPredicateFails()
    {
        // Insert 8 into a sorted list: predicate is "item < 8", so it inserts once item >= 8
        var list = new List<int> { 1, 2, 3, 4, 5, 10, 12 };

        list.InsertWhere(8, x => x < 8);

        Assert.Equal([1, 2, 3, 4, 5, 8, 10, 12], list);
    }

    [Fact]
    public void InsertWhere_PredicateAlwaysTrue_AppendsToEnd()
    {
        var list = new List<int> { 1, 2, 3 };

        list.InsertWhere(99, _ => true);

        Assert.Equal([1, 2, 3, 99], list);
    }

    [Fact]
    public void InsertWhere_EmptyList_AppendsItem()
    {
        var list = new List<int>();

        list.InsertWhere(5, _ => true);

        Assert.Equal([5], list);
    }

    // ------- BinarySearch -------

    [Fact]
    public void BinarySearch_FindsExistingItem()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };

        var result = list.BinarySearch(x => x, 3);

        Assert.Equal(3, result);
    }

    [Fact]
    public void BinarySearch_FindsFirstItem()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };

        var result = list.BinarySearch(x => x, 1);

        Assert.Equal(1, result);
    }

    [Fact]
    public void BinarySearch_FindsLastItem()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };

        var result = list.BinarySearch(x => x, 5);

        Assert.Equal(5, result);
    }

    [Fact]
    public void BinarySearch_FindsItemByCustomKey()
    {
        var list = new List<(int Id, string Name)>
        {
            (1, "Alice"),
            (2, "Bob"),
            (3, "Charlie")
        };

        var result = list.BinarySearch(x => x.Id, 2);

        Assert.Equal((2, "Bob"), result);
    }

    [Fact]
    public void BinarySearch_ItemNotFound_ThrowsInvalidOperationException()
    {
        // Search for a value less than all elements to hit the throw path safely
        var list = new List<int> { 1, 2, 3, 4, 5 };

        Assert.Throws<InvalidOperationException>(() => list.BinarySearch(x => x, 0));
    }

    [Fact]
    public void Swap_ExchangesElements()
    {
        var list = new List<int> { 10, 20, 30 };

        list.Swap(0, 2);

        Assert.Equal([30, 20, 10], list);
    }

    [Fact]
    public void Swap_SameIndex_NoOp()
    {
        var list = new List<int> { 1, 2, 3 };

        list.Swap(1, 1);

        Assert.Equal([1, 2, 3], list);
    }

    [Fact]
    public void Swap_OutOfRange_Throws()
    {
        var list = new List<int> { 1, 2 };

        Assert.Throws<ArgumentOutOfRangeException>(() => list.Swap(0, 5));
    }

    [Fact]
    public void Move_ShiftsElement()
    {
        var list = new List<string> { "a", "b", "c", "d" };

        list.Move(0, 2);

        Assert.Equal(["b", "c", "a", "d"], list);
    }

    [Fact]
    public void Move_BackwardsShift()
    {
        var list = new List<string> { "a", "b", "c", "d" };

        list.Move(3, 0);

        Assert.Equal(["d", "a", "b", "c"], list);
    }

    [Fact]
    public void Move_SameIndex_NoOp()
    {
        var list = new List<string> { "a", "b" };

        list.Move(0, 0);

        Assert.Equal(["a", "b"], list);
    }

    [Fact]
    public void Move_OutOfRange_Throws()
    {
        var list = new List<int> { 1 };

        Assert.Throws<ArgumentOutOfRangeException>(() => list.Move(0, 5));
    }
}
