using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;

namespace AgentFrameworkToolkit;

/// <summary>
/// Configuration of Logging Middleware
/// </summary>
/// <param name="loggerFactory">A LoggerFactory to use with the Middleware</param>
/// <param name="configure">Configuration of the Logging</param>
public class LoggingMiddleware(ILoggerFactory? loggerFactory = null, Action<LoggingAgent>? configure = null)
{
    /// <summary>
    /// A LoggerFactory to use with the Middleware
    /// </summary>
    public ILoggerFactory? LoggerFactory { get; } = loggerFactory;

    /// <summary>
    /// Configuration of the Logging
    /// </summary>
    public Action<LoggingAgent>? Configure { get; } = configure;
}
