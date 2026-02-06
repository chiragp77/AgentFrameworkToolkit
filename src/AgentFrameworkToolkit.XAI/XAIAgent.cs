using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.XAI;

/// <summary>
/// An Agent targeting XAI
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class XAIAgent(AIAgent innerAgent) : Agent(innerAgent);
