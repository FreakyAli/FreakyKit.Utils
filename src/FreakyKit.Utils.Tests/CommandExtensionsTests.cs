namespace FreakyKit.Utils.Tests;

public class CommandExtensionsTests
{
    private sealed class TestCommand(bool canExecute) : ICommand
    {
        public bool Executed { get; private set; }
        public object? LastParameter { get; private set; }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => canExecute;

        public void Execute(object? parameter)
        {
            Executed = true;
            LastParameter = parameter;
        }
    }

    [Fact]
    public void ExecuteWhenAvailable_WhenCanExecute_ExecutesCommand()
    {
        var command = new TestCommand(canExecute: true);

        command.ExecuteWhenAvailable();

        Assert.True(command.Executed);
    }

    [Fact]
    public void ExecuteWhenAvailable_WhenCannotExecute_DoesNotExecute()
    {
        var command = new TestCommand(canExecute: false);

        command.ExecuteWhenAvailable();

        Assert.False(command.Executed);
    }

    [Fact]
    public void ExecuteWhenAvailable_PassesParameterToCommand()
    {
        var command = new TestCommand(canExecute: true);

        command.ExecuteWhenAvailable("test-param");

        Assert.Equal("test-param", command.LastParameter);
    }

    [Fact]
    public void ExecuteWhenAvailable_NullCommand_DoesNotThrow()
    {
        ICommand? command = null;

        // Null-conditional in the implementation makes this safe
        command.ExecuteWhenAvailable();
    }
}
