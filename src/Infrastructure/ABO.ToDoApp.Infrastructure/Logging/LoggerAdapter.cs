using ABO.ToDoApp.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace ABO.ToDoApp.Infrastructure.Logging;

public class LoggerAdapter<T> : ILoggerAdapter<T> where T : class
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<T>();
    }

    public void LogError(Exception? exception, string? message, params object?[] args)
    {
        _logger.LogError(exception, message, args);
    }

    public void LogInformation(string? message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }
}
