using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AgentFrameworkToolkit;

/// <summary>
/// Various MiddlewareDelegates
/// </summary>
public class MiddlewareDelegates
{
    /// <summary>
    /// Tool calling Middleware Delegate
    /// </summary>
    public delegate ValueTask<object?> ToolCallingMiddlewareDelegate(
        AIAgent callingAgent,
        FunctionInvocationContext context,
        Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next,
        CancellationToken cancellationToken);
}
