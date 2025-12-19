using AgentFrameworkToolkit.OpenAI;
using Microsoft.Agents.AI;

#pragma warning disable OPENAI001

namespace Samples.Providers;

public static class OpenAI
{
    public static async Task RunAsync()
    {
        Secrets secrets = SecretsManager.GetConfiguration();
        OpenAIAgentFactory factory = new(new OpenAIConnection
        {
            ApiKey = secrets.OpenAiApiKey,
            DefaultClientType = ClientType.ResponsesApi
        });

        OpenAIAgent agent = factory.CreateAgent(new AgentOptions
        {
            Id = "1111",
            ClientType = ClientType.ResponsesApi,
            ReasoningEffort = OpenAIReasoningEffort.High,
            ReasoningSummaryVerbosity = OpenAIReasoningSummaryVerbosity.Concise,
            Model = OpenAIChatModels.Gpt5Mini,
            MaxOutputTokens = 2000,
            RawHttpCallDetails = details => { Console.WriteLine(details.RequestData); },
        });

        AgentRunResponse response = await agent.RunAsync("Hello");
        Console.WriteLine(response);
    }
}
