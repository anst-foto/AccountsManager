namespace AccountsManager.Logger;

public class LogToConsole : ILogger
{
    private static void Print(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    public void Info(string message)
    {
        Print($"{DateTime.Now:g} [{nameof(Info)}]: {message}", ConsoleColor.Blue);
    }
}