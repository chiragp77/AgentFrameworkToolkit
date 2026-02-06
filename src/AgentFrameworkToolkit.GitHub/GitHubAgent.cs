using Microsoft.Agents.AI;
using JetBrains.Annotations;

namespace AgentFrameworkToolkit.GitHub;

/// <summary>
/// An Agent targeting GitHub Models
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class GitHubAgent(AIAgent innerAgent) : Agent(innerAgent);
