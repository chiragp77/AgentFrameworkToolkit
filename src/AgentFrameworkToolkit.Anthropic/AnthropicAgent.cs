using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.Anthropic;

/// <summary>
/// An Agent targeting Anthropic (Claude)
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class AnthropicAgent(AIAgent innerAgent) : Agent(innerAgent);
