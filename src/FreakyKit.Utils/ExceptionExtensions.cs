namespace FreakyKit.Utils;

public static class ExceptionExtensions
{
    /// <summary>
    /// Writes <paramref name="exception"/> and every <see cref="Exception.InnerException"/> in its chain
    /// (message + stack trace) to <see cref="Trace"/> via a single <see cref="Trace.TraceError(string)"/> call.
    /// </summary>
    /// <param name="exception">The exception whose chain should be traced.</param>
    public static void TraceException(this Exception exception)
    {
        var stringBuilder = new StringBuilder();
        Exception? current = exception;
        while (current is not null)
        {
            stringBuilder.AppendLine(current.Message);
            stringBuilder.AppendLine(current.StackTrace);
            current = current.InnerException;
        }
        Trace.TraceError(stringBuilder.ToString());
    }

    /// <summary>
    /// Walks the <see cref="Exception.InnerException"/> chain and returns the innermost exception.
    /// Returns <paramref name="exception"/> itself when it has no inner.
    /// </summary>
    /// <param name="exception">The starting exception.</param>
    public static Exception GetRootCause(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        var current = exception;
        while (current.InnerException is not null)
            current = current.InnerException;
        return current;
    }

    /// <summary>
    /// Concatenates the <see cref="Exception.Message"/> of <paramref name="exception"/> and every nested
    /// <see cref="Exception.InnerException"/>, joined by <paramref name="separator"/>.
    /// </summary>
    /// <param name="exception">The exception whose chain to summarize.</param>
    /// <param name="separator">Separator inserted between messages. Defaults to <c>" -> "</c>.</param>
    public static string GetAllMessages(this Exception exception, string separator = " -> ")
    {
        ArgumentNullException.ThrowIfNull(exception);
        var messages = new List<string>();
        Exception? current = exception;
        while (current is not null)
        {
            messages.Add(current.Message);
            current = current.InnerException;
        }
        return string.Join(separator, messages);
    }
}