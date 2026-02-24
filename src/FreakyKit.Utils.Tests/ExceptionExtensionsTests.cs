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
}
