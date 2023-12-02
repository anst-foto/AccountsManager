namespace AccountsManager.Logger;

public class LogToFile : ILogger
{
    private readonly string _path;

    public LogToFile(string path)
    {
        _path = path;
    }

    private void WriteToFile(string message)
    {
        using var file = new StreamWriter(path: _path, append: true);
        file.WriteLine(message);
    }
    
    public void Info(string message)
    {
        WriteToFile($"{DateTime.Now:g} [{nameof(Info)}]: {message}");
    }
}