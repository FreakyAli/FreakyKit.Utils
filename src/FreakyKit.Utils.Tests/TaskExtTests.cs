namespace FreakyKit.Utils.Tests;

public class TaskExtTests
{
    // ------- RunConcurrently -------

    [Fact]
    public void RunConcurrently_NullTask_ThrowsArgumentNullException()
    {
        Task task = null!;

        Assert.Throws<ArgumentNullException>(() => task.RunConcurrently());
    }

    [Fact]
    public async Task RunConcurrently_CreatedTask_StartsAndCompletes()
    {
        bool executed = false;
        var task = new Task(() => { executed = true; });

        task.RunConcurrently();

        await Task.Delay(200, TestContext.Current.CancellationToken); // give the task time to run
        Assert.True(executed);
    }

    [Fact]
    public void RunConcurrently_AlreadyRunningTask_DoesNotThrow()
    {
        var task = Task.Run(() => Task.Delay(50, TestContext.Current.CancellationToken), TestContext.Current.CancellationToken);

        // Should not throw — only calls Start() when status is Created
        task.RunConcurrently();
    }

    // ------- WhenAll -------

    [Fact]
    public async Task WhenAll_AllTasksSucceed_ReturnsAllResults()
    {
        var t1 = Task.FromResult(1);
        var t2 = Task.FromResult(2);
        var t3 = Task.FromResult(3);

        var results = await TaskExt.WhenAll(t1, t2, t3);

        Assert.Equal([1, 2, 3], results.OrderBy(x => x).ToList());
    }

    [Fact]
    public async Task WhenAll_OneTaskFaults_ThrowsAggregateException()
    {
        var t1 = Task.FromResult(1);
        var t2 = Task.FromException<int>(new InvalidOperationException("failed"));

        await Assert.ThrowsAsync<AggregateException>(async () =>
            await TaskExt.WhenAll(t1, t2));
    }

    [Fact]
    public async Task WhenAll_MultipleTasksFault_AggregateExceptionContainsAll()
    {
        var t1 = Task.FromException<int>(new InvalidOperationException("error 1"));
        var t2 = Task.FromException<int>(new ArgumentException("error 2"));

        var ex = await Assert.ThrowsAsync<AggregateException>(async () =>
            await TaskExt.WhenAll(t1, t2));

        Assert.Equal(2, ex.InnerExceptions.Count);
    }

    // ------- WithAggregateException (non-generic) -------

    [Fact]
    public async Task WithAggregateException_CompletedTask_DoesNotThrow()
    {
        var task = Task.CompletedTask;

        await task.WithAggregateException(); // should not throw
    }

    [Fact]
    public async Task WithAggregateException_CanceledTask_ThrowsOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var task = Task.FromCanceled(cts.Token);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WithAggregateException());
    }

    // ------- WithAggregateException<T> (generic) -------

    [Fact]
    public async Task WithAggregateException_Generic_SuccessfulTask_ReturnsResult()
    {
        var task = Task.FromResult(42);

        var result = await task.WithAggregateException();

        Assert.Equal(42, result);
    }

    // ------- TimeoutAfter<TResult> -------

    [Fact]
    public async Task TimeoutAfter_CompletesBeforeTimeout_ReturnsResult()
    {
        var task = Task.FromResult(42);

        var result = await task.TimeoutAfter<int>(TimeSpan.FromSeconds(5));

        Assert.Equal(42, result);
    }

    [Fact]
    public async Task TimeoutAfter_ExceedsTimeout_ThrowsTimeoutException()
    {
        // A task that takes 10 seconds — far longer than the 50 ms timeout
        var task = Task.Delay(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken).ContinueWith(_ => 0);

        await Assert.ThrowsAsync<TimeoutException>(async () =>
            await task.TimeoutAfter<int>(TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task FireAndForget_FailedTask_InvokesHandler()
    {
        Exception? captured = null;
        var task = Task.Run(() => throw new InvalidOperationException("boom"), TestContext.Current.CancellationToken);

        task.FireAndForget(ex => captured = ex);

        // Wait for the continuation to fire
        for (int i = 0; i < 50 && captured is null; i++)
            await Task.Delay(20, TestContext.Current.CancellationToken);

        Assert.IsType<AggregateException>(captured);
        Assert.Contains(captured!.InnerException?.Message ?? "", "boom");
    }

    [Fact]
    public async Task FireAndForget_NoHandler_DoesNotPropagate()
    {
        var task = Task.Run(() => throw new InvalidOperationException("ignored"), TestContext.Current.CancellationToken);

        task.FireAndForget();

        // Give the continuation time to drain
        await Task.Delay(50, TestContext.Current.CancellationToken);
        // If no handler, exception is observed but discarded — test simply confirms no throw on this thread.
    }

    [Fact]
    public async Task FireAndForget_NullTask_Throws()
    {
        await Task.Yield();
        Task task = null!;

        Assert.Throws<ArgumentNullException>(() => task.FireAndForget());
    }

    [Fact]
    public async Task Retry_SucceedsOnSecondAttempt()
    {
        int attempts = 0;
        Func<Task> action = async () =>
        {
            attempts++;
            await Task.Yield();
            if (attempts < 2) throw new InvalidOperationException("fail");
        };

        await action.Retry(3, TimeSpan.Zero);

        Assert.Equal(2, attempts);
    }

    [Fact]
    public async Task Retry_AllAttemptsFail_ThrowsLast()
    {
        int attempts = 0;
        Func<Task> action = async () =>
        {
            attempts++;
            await Task.Yield();
            throw new InvalidOperationException($"attempt {attempts}");
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => action.Retry(3, TimeSpan.Zero));

        Assert.Equal(3, attempts);
        Assert.Equal("attempt 3", ex.Message);
    }

    [Fact]
    public async Task Retry_ShouldRetryFalse_StopsImmediately()
    {
        int attempts = 0;
        Func<Task> action = async () =>
        {
            attempts++;
            await Task.Yield();
            throw new InvalidOperationException("nope");
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            action.Retry(5, TimeSpan.Zero, _ => false));

        Assert.Equal(1, attempts);
    }

    [Fact]
    public async Task Retry_Generic_ReturnsResult()
    {
        int attempts = 0;
        Func<Task<int>> action = async () =>
        {
            attempts++;
            await Task.Yield();
            if (attempts < 2) throw new InvalidOperationException();
            return 42;
        };

        var result = await action.Retry(3, TimeSpan.Zero);

        Assert.Equal(42, result);
        Assert.Equal(2, attempts);
    }

    [Fact]
    public async Task Retry_InvalidMaxAttempts_Throws()
    {
        Func<Task> action = () => Task.CompletedTask;

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => action.Retry(0, TimeSpan.Zero));
    }

    [Fact]
    public async Task WithCancellation_CompletedFirst_ReturnsResult()
    {
        var cts = new CancellationTokenSource();
        var task = Task.FromResult(123);

        var result = await task.WithCancellation(cts.Token);

        Assert.Equal(123, result);
    }

    [Fact]
    public async Task WithCancellation_CancelledFirst_Throws()
    {
        using var cts = new CancellationTokenSource();
        var task = Task.Delay(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);

        cts.CancelAfter(50);

        await Assert.ThrowsAsync<OperationCanceledException>(() => task.WithCancellation(cts.Token));
    }

    [Fact]
    public async Task WithCancellation_Generic_CancelledFirst_Throws()
    {
        using var cts = new CancellationTokenSource();
        var task = Task.Delay(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken).ContinueWith(_ => 99);

        cts.CancelAfter(50);

        await Assert.ThrowsAsync<OperationCanceledException>(() => task.WithCancellation(cts.Token));
    }
}
