namespace FreakyKit.Utils.Tests;

public class ExceptionExtensionsTests
{
    [Fact]
    public void TraceException_SimpleException_DoesNotThrow()
    {
        var exception = new InvalidOperationException("something went wrong");

        exception.TraceException();
    }

    [Fact]
    public void TraceException_ExceptionWithInnerException_DoesNotThrow()
    {
        var inner = new ArgumentNullException("param", "param was null");
        var outer = new InvalidOperationException("outer message", inner);

        outer.TraceException();
    }

    [Fact]
    public void TraceException_DeepInnerExceptionChain_DoesNotThrow()
    {
        var level3 = new Exception("level 3");
        var level2 = new Exception("level 2", level3);
        var level1 = new Exception("level 1", level2);

        level1.TraceException();
    }

    [Fact]
    public void GetRootCause_NoInner_ReturnsSelf()
    {
        var ex = new InvalidOperationException("solo");

        Assert.Same(ex, ex.GetRootCause());
    }

    [Fact]
    public void GetRootCause_DeepChain_ReturnsInnermost()
    {
        var leaf = new ArgumentException("leaf");
        var mid = new InvalidOperationException("mid", leaf);
        var root = new Exception("root", mid);

        Assert.Same(leaf, root.GetRootCause());
    }

    [Fact]
    public void GetAllMessages_NoInner_ReturnsSingle()
    {
        var ex = new Exception("only");

        Assert.Equal("only", ex.GetAllMessages());
    }

    [Fact]
    public void GetAllMessages_DeepChain_JoinsAll()
    {
        var leaf = new Exception("leaf");
        var mid = new Exception("mid", leaf);
        var root = new Exception("root", mid);

        Assert.Equal("root -> mid -> leaf", root.GetAllMessages());
    }

    [Fact]
    public void GetAllMessages_CustomSeparator()
    {
        var leaf = new Exception("leaf");
        var root = new Exception("root", leaf);

        Assert.Equal("root | leaf", root.GetAllMessages(" | "));
    }
}
