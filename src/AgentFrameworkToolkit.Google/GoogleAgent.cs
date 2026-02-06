using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.Google;

/// <summary>
/// An Agent targeting Google
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class GoogleAgent(AIAgent innerAgent) : Agent(innerAgent);
