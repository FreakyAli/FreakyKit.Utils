namespace FreakyKit.Utils;

public static class ExceptionExtensions
{
    public static void TraceException(this Exception exception)
    {
        var stringBuilder = new StringBuilder();
        while (exception is not null)
        {
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);
            exception = exception.InnerException;
        }
        Trace.TraceError(stringBuilder.ToString());
    }
}