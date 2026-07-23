namespace FreakyKit.Utils;

public static class CommandExtensions
{
    /// <summary>
    /// Invokes <see cref="ICommand.Execute(object)"/> only when <paramref name="command"/> is non-null
    /// and <see cref="ICommand.CanExecute(object)"/> returns <c>true</c>.
    /// </summary>
    /// <param name="command">The command to invoke; may be <c>null</c>.</param>
    /// <param name="parameter">Optional parameter passed to <c>CanExecute</c>/<c>Execute</c>.</param>
    public static void ExecuteWhenAvailable(this ICommand? command, object? parameter = null)
    {
        if (command?.CanExecute(parameter) == true)
        {
            command?.Execute(parameter);
        }
    }
}
