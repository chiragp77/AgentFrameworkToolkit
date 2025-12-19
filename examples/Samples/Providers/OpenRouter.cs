using AgentFrameworkToolkit.OpenRouter;
using Microsoft.Agents.AI;

namespace Samples.Providers;

public static class OpenRouter
{
    public static async Task RunAsync()
    {
        Secrets secrets = SecretsManager.GetConfiguration();
        OpenRouterAgentFactory factory = new(new OpenRouterConnection
        {
            ApiKey = secrets.OpenRouterApiKey
        });

        OpenRouterAgent agent = factory.CreateAgent(OpenRouterChatModels.Google.Gemini25Flash);

        AgentRunResponse response = await agent.RunAsync("Hello");
        Console.WriteLine(response);
    }
}
