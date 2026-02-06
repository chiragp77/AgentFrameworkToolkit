using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.Mistral;

/// <summary>
/// An Agent targeting Mistral
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class MistralAgent(AIAgent innerAgent) : Agent(innerAgent);
