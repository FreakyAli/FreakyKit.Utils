namespace FreakyKit.Utils;

public static class CommandExtensions
{
    public static void ExecuteWhenAvailable(this ICommand command, object? parameter = null)
    {
        if (command?.CanExecute(parameter) == true)
        {
            command?.Execute(parameter);
        }
    }
}
