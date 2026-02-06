using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.Cohere;

/// <summary>
/// An Agent targeting Cohere
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class CohereAgent(AIAgent innerAgent) : Agent(innerAgent);
