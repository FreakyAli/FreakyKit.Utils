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
    public void Append_AddsElementToEnd()
    {
        IEnumerable<int> source = [1, 2, 3];

        var result = EnumerableExtensions.Append(source, 4).ToList();

        Assert.Equal([1, 2, 3, 4], result);
    }

    [Fact]
    public void Prepend_AddsElementToStart()
    {
        IEnumerable<int> source = [2, 3, 4];

        var result = EnumerableExtensions.Prepend(source, 1).ToList();

        Assert.Equal([1, 2, 3, 4], result);
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
}
