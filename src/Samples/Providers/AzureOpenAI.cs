using AgentFrameworkToolkit.AzureOpenAI;
using AgentFrameworkToolkit.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;

namespace Samples.Providers;

public static class AzureOpenAI
{
    public static async Task RunAsync()
    {
        Configuration configuration = ConfigurationManager.GetConfiguration();
        AzureOpenAIAgentFactory factory = new(new AzureOpenAIConnection
        {
            Endpoint = configuration.AzureOpenAiEndpoint,
            Credentials = new AzureCliCredential()
        });

        AzureOpenAIAgent agent = factory.CreateAgent(new OpenAIAgentOptionsForChatClientWithoutReasoning
        {
            Model = OpenAIChatModels.Gpt41Mini,
            Instructions = "Speak like a pirate"
        });

        AgentRunResponse response = await agent.RunAsync<string>("Hello");
        Console.WriteLine(response);
    }
}