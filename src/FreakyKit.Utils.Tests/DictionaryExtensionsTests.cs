namespace FreakyKit.Utils.Tests;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetOrAdd_Value_KeyMissing_AddsAndReturns()
    {
        var dict = new Dictionary<string, int>();

        var result = dict.GetOrAdd("a", 42);

        Assert.Equal(42, result);
        Assert.Equal(42, dict["a"]);
    }

    [Fact]
    public void GetOrAdd_Value_KeyPresent_ReturnsExisting_DoesNotOverwrite()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };

        var result = dict.GetOrAdd("a", 99);

        Assert.Equal(1, result);
        Assert.Equal(1, dict["a"]);
    }

    [Fact]
    public void GetOrAdd_Factory_OnlyInvokedWhenKeyMissing()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };
        int factoryCalls = 0;

        var existing = dict.GetOrAdd("a", _ => { factoryCalls++; return 99; });
        var added = dict.GetOrAdd("b", k => { factoryCalls++; return k.Length; });

        Assert.Equal(1, existing);
        Assert.Equal(1, added);
        Assert.Equal(1, factoryCalls);
    }

    [Fact]
    public void AddOrUpdate_AddsWhenAbsent()
    {
        var dict = new Dictionary<string, int>();

        var result = dict.AddOrUpdate("k", 10, (_, old) => old + 1);

        Assert.Equal(10, result);
        Assert.Equal(10, dict["k"]);
    }

    [Fact]
    public void AddOrUpdate_InvokesUpdaterWhenPresent()
    {
        var dict = new Dictionary<string, int> { ["k"] = 5 };

        var result = dict.AddOrUpdate("k", 10, (_, old) => old + 100);

        Assert.Equal(105, result);
        Assert.Equal(105, dict["k"]);
    }

    [Fact]
    public void Merge_CopiesEntries_OverwritingDuplicates()
    {
        var dest = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        var src = new Dictionary<string, int> { ["b"] = 20, ["c"] = 3 };

        dest.Merge(src);

        Assert.Equal(1, dest["a"]);
        Assert.Equal(20, dest["b"]);
        Assert.Equal(3, dest["c"]);
    }

    [Fact]
    public void Invert_SwapsKeyAndValue()
    {
        var dict = new Dictionary<int, string> { [1] = "one", [2] = "two" };

        var inverted = dict.Invert();

        Assert.Equal(1, inverted["one"]);
        Assert.Equal(2, inverted["two"]);
    }

    [Fact]
    public void Invert_DuplicateValues_Throws()
    {
        var dict = new Dictionary<int, string> { [1] = "x", [2] = "x" };

        Assert.Throws<ArgumentException>(() => dict.Invert());
    }

    [Fact]
    public void ToReadOnlyDictionary_SnapshotIsIndependent()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };

        var ro = dict.ToReadOnlyDictionary();
        dict["a"] = 999;

        Assert.Equal(1, ro["a"]);
    }

    [Fact]
    public void GetOrAdd_NullDictionary_Throws()
    {
        IDictionary<string, int> dict = null!;

        Assert.Throws<ArgumentNullException>(() => dict.GetOrAdd("a", 1));
    }
}
