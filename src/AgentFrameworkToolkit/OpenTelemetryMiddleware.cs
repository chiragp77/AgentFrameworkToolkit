using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit;

/// <summary>
/// Configuration of OpenTelemetry Middleware
/// </summary>
/// <param name="source">The OpenTelemetry Source</param>
/// <param name="configure">The OpenTelemetry Configuration</param>
public class OpenTelemetryMiddleware(string? source, Action<OpenTelemetryAgent> configure)
{
    /// <summary>
    /// The OpenTelemetry Source
    /// </summary>
    public string? Source { get; } = source;

    /// <summary>
    /// The OpenTelemetry Configuration
    /// </summary>
    public Action<OpenTelemetryAgent> Configure { get; } = configure;
}
