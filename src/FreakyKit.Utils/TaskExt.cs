namespace FreakyKit.Utils;

public static class TaskExt
{
    /// <summary>
    /// Runs the Task in a concurrent thread without waiting for it to complete. This will start the task if it is not already running.
    /// </summary>
    /// <param name="task">The task to run.</param>
    /// <remarks>This is usually used to avoid warning messages about not waiting for the task to complete.</remarks>
    public static void RunConcurrently(this Task task)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task), "task is null.");

        if (task.Status == TaskStatus.Created)
            task.Start();
    }
    
    /// <summary>
    /// A version of WhenAll that throws all the exceptions encountered!
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

    /// <summary>
    /// Awaits <paramref name="source"/> and forces its <see cref="AggregateException"/> to propagate on failure
    /// (instead of <c>await</c>'s usual single-inner-exception unwrapping). <see cref="TaskCanceledException"/>
    /// is preserved unchanged.
    /// </summary>
    /// <param name="source">The task to await.</param>
    public static async Task WithAggregateException(this Task source)
    {
        try { await source.ConfigureAwait(false); }
        catch when (source.IsCanceled) { throw; }
        catch { source.Wait(); }
    }

    /// <summary>
    /// Generic overload of <see cref="WithAggregateException(Task)"/>: awaits the task and forces its
    /// <see cref="AggregateException"/> to propagate on failure, preserving cancellation.
    /// </summary>
    /// <typeparam name="T">Result type of the task.</typeparam>
    /// <param name="source">The task to await.</param>
    public static async Task<T> WithAggregateException<T>(this Task<T> source)
    {
        try { return await source.ConfigureAwait(false); }
        catch when (source.IsCanceled) { throw; }
        catch { return source.Result; }
    }

    /// <summary>
    /// Awaits <paramref name="task"/> with a hard deadline. Throws <see cref="TimeoutException"/> if the
    /// timeout elapses before the task completes; otherwise returns the task's result.
    /// </summary>
    /// <typeparam name="TResult">Result type of the task.</typeparam>
    /// <param name="task">The task to await.</param>
    /// <param name="timeout">Maximum time to wait.</param>
    /// <exception cref="TimeoutException">Thrown when <paramref name="task"/> does not complete in time.</exception>
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

    /// <summary>
    /// Non-generic overload of <see cref="TimeoutAfter{TResult}(Task{TResult}, TimeSpan)"/>. Throws
    /// <see cref="TimeoutException"/> when <paramref name="task"/> does not complete within <paramref name="timeout"/>.
    /// </summary>
    /// <param name="task">The task to await.</param>
    /// <param name="timeout">Maximum time to wait.</param>
    /// <exception cref="TimeoutException">Thrown when <paramref name="task"/> does not complete in time.</exception>
    public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
        if (completedTask == task)
        {
            timeoutCancellationTokenSource.Cancel();
            await task;
            return;
        }
        throw new TimeoutException();
    }

    /// <summary>
    /// Starts (or observes) <paramref name="task"/> without awaiting it and silently routes any failure to
    /// <paramref name="onException"/> (if supplied). Cancellation is treated as a failure too.
    /// </summary>
    /// <remarks>
    /// Use only when the caller is genuinely indifferent to the result. Unhandled exceptions never propagate.
    /// </remarks>
    /// <param name="task">The task to detach from.</param>
    /// <param name="onException">Optional handler invoked on failure.</param>
    public static void FireAndForget(this Task task, Action<Exception>? onException = null)
    {
        ArgumentNullException.ThrowIfNull(task);
        _ = task.ContinueWith(t =>
        {
            if (t.Exception is { } ex) onException?.Invoke(ex);
        }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
    }

    /// <summary>
    /// Invokes <paramref name="action"/> up to <paramref name="maxAttempts"/> times, waiting
    /// <paramref name="delay"/> between attempts. Stops early if <paramref name="shouldRetry"/> returns <c>false</c>
    /// for the thrown exception. Re-throws the last exception when all attempts fail.
    /// </summary>
    /// <param name="action">Factory producing the task to attempt.</param>
    /// <param name="maxAttempts">Maximum number of attempts (must be ≥ 1).</param>
    /// <param name="delay">Wait between attempts. Zero or negative means no delay.</param>
    /// <param name="shouldRetry">Predicate inspecting the exception to decide whether to retry. Default: retry every exception.</param>
    public static async Task Retry(this Func<Task> action, int maxAttempts, TimeSpan delay, Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (maxAttempts < 1) throw new ArgumentOutOfRangeException(nameof(maxAttempts));

        Exception? last = null;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await action().ConfigureAwait(false);
                return;
            }
            catch (Exception ex) when (shouldRetry?.Invoke(ex) ?? true)
            {
                last = ex;
                if (attempt < maxAttempts && delay > TimeSpan.Zero)
                    await Task.Delay(delay).ConfigureAwait(false);
            }
        }
        throw last!;
    }

    /// <summary>
    /// Generic overload of <see cref="Retry(Func{Task}, int, TimeSpan, Func{Exception, bool}?)"/> for
    /// task factories that produce a result.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="action">Factory producing the task to attempt.</param>
    /// <param name="maxAttempts">Maximum number of attempts (must be ≥ 1).</param>
    /// <param name="delay">Wait between attempts. Zero or negative means no delay.</param>
    /// <param name="shouldRetry">Predicate inspecting the exception to decide whether to retry.</param>
    public static async Task<T> Retry<T>(this Func<Task<T>> action, int maxAttempts, TimeSpan delay, Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (maxAttempts < 1) throw new ArgumentOutOfRangeException(nameof(maxAttempts));

        Exception? last = null;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                return await action().ConfigureAwait(false);
            }
            catch (Exception ex) when (shouldRetry?.Invoke(ex) ?? true)
            {
                last = ex;
                if (attempt < maxAttempts && delay > TimeSpan.Zero)
                    await Task.Delay(delay).ConfigureAwait(false);
            }
        }
        throw last!;
    }

    /// <summary>
    /// Awaits <paramref name="task"/> with external cancellation. If <paramref name="token"/> is cancelled
    /// before the task completes, throws <see cref="OperationCanceledException"/> immediately; the underlying
    /// task continues to run unobserved.
    /// </summary>
    /// <param name="task">The task to await.</param>
    /// <param name="token">Cancellation token.</param>
    public static async Task WithCancellation(this Task task, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(task);
        var tcs = new TaskCompletionSource<bool>();
        using (token.Register(() => tcs.TrySetResult(true)))
        {
            var completed = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);
            if (completed == tcs.Task) throw new OperationCanceledException(token);
        }
        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Generic overload of <see cref="WithCancellation(Task, CancellationToken)"/> that returns the task's result.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="task">The task to await.</param>
    /// <param name="token">Cancellation token.</param>
    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(task);
        var tcs = new TaskCompletionSource<bool>();
        using (token.Register(() => tcs.TrySetResult(true)))
        {
            var completed = await Task.WhenAny(task, tcs.Task).ConfigureAwait(false);
            if (completed == tcs.Task) throw new OperationCanceledException(token);
        }
        return await task.ConfigureAwait(false);
    }
}
