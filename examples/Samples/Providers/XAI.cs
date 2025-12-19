using AgentFrameworkToolkit.OpenAI;
using AgentFrameworkToolkit.XAI;
using Microsoft.Agents.AI;

namespace Samples.Providers;

public static class XAI
{
    public static async Task RunAsync()
    {
        Secrets secrets = SecretsManager.GetConfiguration();
        XAIAgentFactory factory = new(new XAIConnection
        {
            ApiKey = secrets.XAiGrokApiKey,
        });

        XAIAgent agent = factory.CreateAgent(new AgentOptions
        {
            Model = XAIChatModels.Grok4FastNonReasoning,
        });

        AgentRunResponse response = await agent.RunAsync("Hello");
        Console.WriteLine(response);
    }
}
