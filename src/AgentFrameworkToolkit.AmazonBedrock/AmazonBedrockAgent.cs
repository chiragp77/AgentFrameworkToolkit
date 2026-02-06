using JetBrains.Annotations;
using Microsoft.Agents.AI;

namespace AgentFrameworkToolkit.AmazonBedrock;

/// <summary>
/// An Agent targeting Amazon Bedrock
/// </summary>
/// <param name="innerAgent">The inner generic Agent</param>
[PublicAPI]
public class AmazonBedrockAgent(AIAgent innerAgent) : Agent(innerAgent);
