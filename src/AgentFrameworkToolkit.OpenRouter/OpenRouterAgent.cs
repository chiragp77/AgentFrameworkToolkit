using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.OpenRouter;

/// <summary>
/// An Agent targeting OpenRouter
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class OpenRouterAgent(AIAgent innerAgent) : Agent(innerAgent);
