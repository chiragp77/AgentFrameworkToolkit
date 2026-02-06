using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.OpenAI;

/// <summary>
/// An Agent targeting OpenAI
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class OpenAIAgent(AIAgent innerAgent) : Agent(innerAgent);
