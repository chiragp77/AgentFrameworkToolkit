using AgentFrameworkToolkit.AzureOpenAI;
using AgentFrameworkToolkit.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

#pragma warning disable OPENAI001

namespace Samples.Providers;

public static class AzureOpenAI
{
    public static async Task RunAsync()
    {
        Configuration configuration = ConfigurationManager.GetConfiguration();
        AzureOpenAIConnection connection = new()
        {
            Endpoint = configuration.AzureOpenAiEndpoint,
            ApiKey = configuration.AzureOpenAiKey
        };

        AzureOpenAIEmbeddingFactory azureOpenAIEmbeddingFactory = new(connection);

        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = azureOpenAIEmbeddingFactory.GetEmbeddingGenerator("text-embedding-3-small");
        GeneratedEmbeddings<Embedding<float>> generatedEmbeddings = await embeddingGenerator.GenerateAsync(["Hello"]);

        AzureOpenAIAgentFactory factory = new(connection);

        AzureOpenAIAgent agent = factory.CreateAgent(new OpenAIAgentOptionsForChatClientWithReasoning
        {
            Model = OpenAIChatModels.Gpt41Mini,
            ReasoningEffort = ChatReasoningEffortLevel.Low,
            Instructions = "Speak like a pirate",
            RawHttpCallDetails = details => Console.WriteLine(details.RequestData)
        });

        AgentRunResponse response = await agent.RunAsync<string>("Hello");
        Console.WriteLine(response);
    }
}