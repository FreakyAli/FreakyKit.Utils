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
            var innerExp = exception.InnerException;
            if (innerExp is not null)
            {
                exception = innerExp;
            }
        }
        Trace.TraceError(stringBuilder.ToString());
    }
}