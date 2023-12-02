using AccountsManager.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ILogger = AccountsManager.Logger.ILogger;

namespace AccountsManager.Config;

public class AppSettings
{
    public ILogger Logger { get; set; }
    public LogLevel LogLevel { get; set; }
    public string ConnectionString { get; set; }


    private static LogLevel GetLogLevel(string level)
    {
        return level switch
        {
            "Trace" => LogLevel.Trace,
            "Debug" => LogLevel.Debug,
            "Information" => LogLevel.Information,
            "Warning" => LogLevel.Warning,
            "Error" => LogLevel.Error,
            "Critical" => LogLevel.Critical,
            _ => LogLevel.None
        };
    }
    
    private static ILogger GetLogger(string logger, string? path = null)
    {
        return logger switch
        {
            "File" => new LogToFile(path),
            "Console" => new LogToConsole(),
            _ => new LogToConsole()
        };
    }
    
    public static AppSettings Init(string path = "appsettings.json")
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path)
            .Build();
        var level = config.GetChildren().First(s => s.Key == "LogLevel").Value;
        var log = config.GetChildren().First(s => s.Key == "LogName").Value;
        var logPath = config.GetChildren().FirstOrDefault(s => s.Key == "LogPath")?.Value;
        
        var connectionString = config.GetConnectionString("DefaultConnection");
        return new AppSettings()
        {
            ConnectionString = connectionString,
            LogLevel = GetLogLevel(level),
            Logger = GetLogger(log, logPath)
        };
    }
}