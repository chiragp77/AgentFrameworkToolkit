using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.AzureOpenAI;

/// <summary>
/// An Agent targeting AzureOpenAI
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class AzureOpenAIAgent(AIAgent innerAgent) : Agent(innerAgent);
