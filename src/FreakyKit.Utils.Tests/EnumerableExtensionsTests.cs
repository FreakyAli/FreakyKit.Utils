namespace FreakyKit.Utils.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void ToObservable_ConvertsList_ReturnsObservableCollection()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = source.ToObservable();

        Assert.IsType<ObservableCollection<int>>(result);
        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void WithIndex_ReturnsItemsWithCorrectIndices()
    {
        IEnumerable<string> source = ["a", "b", "c"];

        var result = source.WithIndex().ToList();

        Assert.Equal(("a", 0), result[0]);
        Assert.Equal(("b", 1), result[1]);
        Assert.Equal(("c", 2), result[2]);
    }

    [Fact]
    public void WithIndex_NullSource_ReturnsEmptyEnumerable()
    {
        IEnumerable<string> source = null!;

        var result = source.WithIndex().ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void IsNullOrEmpty_NullEnumerable_ReturnsTrue()
    {
        IEnumerable<int> source = null!;

        Assert.True(source.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_EmptyEnumerable_ReturnsTrue()
    {
        IEnumerable<int> source = [];

        Assert.True(source.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_NonEmptyEnumerable_ReturnsFalse()
    {
        IEnumerable<int> source = [1, 2, 3];

        Assert.False(source.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_EmptyCollection_UsesCountOptimization()
    {
        IEnumerable<int> source = new List<int>();

        Assert.True(source.IsNullOrEmpty());
    }

    [Fact]
    public void DistinctBy_RemovesDuplicatesByKey()
    {
        var source = new[] { (Id: 1, Name: "Alice"), (Id: 2, Name: "Bob"), (Id: 1, Name: "Duplicate") };

        var result = EnumerableExtensions.DistinctBy(source, x => x.Id).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Name == "Alice");
        Assert.Contains(result, x => x.Name == "Bob");
    }

    [Fact]
    public void DistinctBy_AllUnique_ReturnsAllElements()
    {
        var source = new[] { 1, 2, 3 };

        var result = EnumerableExtensions.DistinctBy(source, x => x).ToList();

        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void ForEach_ExecutesActionForEachElement()
    {
        IEnumerable<int> source = [1, 2, 3];
        var visited = new List<int>();

        source.ForEach(x => visited.Add(x));

        Assert.Equal([1, 2, 3], visited);
    }

    [Fact]
    public void SingleOrDefault_NullSource_ReturnsDefault()
    {
        IEnumerable<int> source = null!;

        var result = EnumerableExtensions.SingleOrDefault(source, x => x == 1, -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void SingleOrDefault_MatchFound_ReturnsItem()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = EnumerableExtensions.SingleOrDefault(source, x => x == 2, -1);

        Assert.Equal(2, result);
    }

    [Fact]
    public void SingleOrDefault_NoMatch_ReturnsDefault()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = EnumerableExtensions.SingleOrDefault(source, x => x == 99, -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void FirstOrDefault_NullSource_ReturnsDefault()
    {
        IEnumerable<int> source = null!;

        var result = EnumerableExtensions.FirstOrDefault(source, x => x > 0, -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void FirstOrDefault_MatchFound_ReturnsFirstMatch()
    {
        IEnumerable<int> source = [1, 2, 3, 4];

        var result = EnumerableExtensions.FirstOrDefault(source, x => x > 2, -1);

        Assert.Equal(3, result);
    }

    [Fact]
    public void FirstOrDefault_NoMatch_ReturnsDefault()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = EnumerableExtensions.FirstOrDefault(source, x => x > 10, -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void ElementAtOrDefault_NullSource_ReturnsDefault()
    {
        IEnumerable<int> source = null!;

        var result = EnumerableExtensions.ElementAtOrDefault(source, 0, -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void ElementAtOrDefault_ValidIndex_ReturnsElement()
    {
        IEnumerable<int> source = [10, 20, 30];

        var result = EnumerableExtensions.ElementAtOrDefault(source, 1, -1);

        Assert.Equal(20, result);
    }

    [Fact]
    public void ElementAtOrDefault_OutOfRange_ReturnsDefault()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = EnumerableExtensions.ElementAtOrDefault(source, 99, -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void EmptyIfNull_NullSource_ReturnsEmptyEnumerable()
    {
        IEnumerable<int> source = null!;

        var result = source.EmptyIfNull();

        Assert.Empty(result);
    }

    [Fact]
    public void EmptyIfNull_NonNullSource_ReturnsSameElements()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = source.EmptyIfNull();

        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void Shuffle_ReturnsSameElements()
    {
        IEnumerable<int> source = [1, 2, 3, 4, 5];

        var result = source.Shuffle().ToList();

        Assert.Equal(5, result.Count);
        Assert.Equal([1, 2, 3, 4, 5], result.Order().ToList());
    }

    [Fact]
    public void Shuffle_NullSource_ThrowsArgumentNullException()
    {
        IEnumerable<int> source = null!;

        Assert.Throws<ArgumentNullException>(() => source.Shuffle().ToList());
    }

    [Fact]
    public void WhereNotNull_ReferenceType_FiltersNulls()
    {
        IEnumerable<string?> source = ["a", null, "b", null, "c"];

        Assert.Equal(["a", "b", "c"], source.WhereNotNull().ToList());
    }

    [Fact]
    public void WhereNotNull_NullableValue_FiltersNulls()
    {
        IEnumerable<int?> source = [1, null, 2, null, 3];

        Assert.Equal([1, 2, 3], source.WhereNotNull().ToList());
    }

    [Fact]
    public void WhereNotNull_NullSource_Throws()
    {
        IEnumerable<string?> source = null!;

        Assert.Throws<ArgumentNullException>(() => source.WhereNotNull().ToList());
    }

    [Fact]
    public void JoinString_JoinsWithSeparator()
    {
        IEnumerable<int> source = [1, 2, 3];

        Assert.Equal("1,2,3", source.JoinString(","));
    }

    [Fact]
    public void JoinString_EmptySource_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, Array.Empty<int>().JoinString(","));
    }

    [Fact]
    public void IndexOf_FindsFirstMatch()
    {
        IEnumerable<int> source = [10, 20, 30, 40];

        Assert.Equal(2, source.IndexOf(x => x == 30));
    }

    [Fact]
    public void IndexOf_NoMatch_ReturnsMinusOne()
    {
        Assert.Equal(-1, new[] { 1, 2, 3 }.IndexOf(x => x == 99));
    }

    [Fact]
    public void None_NoMatch_ReturnsTrue()
    {
        Assert.True(new[] { 1, 2, 3 }.None(x => x > 10));
    }

    [Fact]
    public void None_HasMatch_ReturnsFalse()
    {
        Assert.False(new[] { 1, 2, 3 }.None(x => x == 2));
    }

    [Fact]
    public void None_NullSource_Throws()
    {
        IEnumerable<int>? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.None(x => x > 0));
    }

    [Fact]
    public void None_NullPredicate_Throws()
    {
        IEnumerable<int> source = [1, 2, 3];
        Assert.Throws<ArgumentNullException>(() => source.None(null!));
    }

    [Fact]
    public void Partition_SplitsCorrectly()
    {
        IEnumerable<int> source = [1, 2, 3, 4, 5];

        var (evens, odds) = source.Partition(x => x % 2 == 0);

        Assert.Equal([2, 4], evens);
        Assert.Equal([1, 3, 5], odds);
    }

    [Fact]
    public void Partition_AllMatch_UnmatchedEmpty()
    {
        var (matched, unmatched) = new[] { 1, 2 }.Partition(_ => true);

        Assert.Equal([1, 2], matched);
        Assert.Empty(unmatched);
    }

    [Fact]
    public void TakeRandom_ReturnsCountElements()
    {
        IEnumerable<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        var result = source.TakeRandom(3).ToList();

        Assert.Equal(3, result.Count);
        Assert.All(result, x => Assert.Contains(x, source));
    }

    [Fact]
    public void TakeRandom_CountExceedsLength_ReturnsAll()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = source.TakeRandom(10).ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal([1, 2, 3], result.Order().ToList());
    }

    [Fact]
    public void TakeRandom_NegativeCount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1, 2 }.TakeRandom(-1));
    }

    [Fact]
    public void TakeRandom_ZeroCount_ReturnsEmpty()
    {
        Assert.Empty(new[] { 1, 2, 3 }.TakeRandom(0));
    }
}
