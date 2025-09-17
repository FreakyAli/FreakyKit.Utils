namespace FreakyKit.Utils;

public static class TaskExt
{
    /// <summary>
    /// A verison of WhenAll that throws all the exceptions encountered!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tasks"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            return await allTasks;
        }
        catch
        {
            //purposely ignore since we will get the exceptions from allTasks;
        }
        // Rethrow the first exception 
        // but include all the others as InnerExceptions
        // so that the caller can see everything that went wrong
        // Nullable warning is a false positive here
#pragma warning disable CS8597
        throw allTasks.Exception;
#pragma warning restore CS8597 
    }

    public static async Task WithAggregateException(this Task source)
    {
        try { await source.ConfigureAwait(false); }
        catch when (source.IsCanceled) { throw; }
        catch { source.Wait(); }
    }

    public static async Task<T> WithAggregateException<T>(this Task<T> source)
    {
        try { return await source.ConfigureAwait(false); }
        catch when (source.IsCanceled) { throw; }
        catch { return source.Result; }
    }

    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            return await task;
        }
        throw new TimeoutException();
    }

    public static async Task<TResult> TimeoutAfter<TResult>(this Task task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            await task;
        }
        throw new TimeoutException();
    }
}