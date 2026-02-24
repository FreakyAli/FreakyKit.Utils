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

        await Task.Delay(200); // give the task time to run
        Assert.True(executed);
    }

    [Fact]
    public void RunConcurrently_AlreadyRunningTask_DoesNotThrow()
    {
        var task = Task.Run(() => Task.Delay(50));

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
        var task = Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith(_ => 0);

        await Assert.ThrowsAsync<TimeoutException>(async () =>
            await task.TimeoutAfter<int>(TimeSpan.FromMilliseconds(50)));
    }
}
