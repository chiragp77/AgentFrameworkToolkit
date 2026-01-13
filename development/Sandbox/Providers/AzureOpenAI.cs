using AgentFrameworkToolkit;
using AgentFrameworkToolkit.AzureOpenAI;
using AgentFrameworkToolkit.OpenAI;
using AgentFrameworkToolkit.OpenRouter;
using AgentFrameworkToolkit.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using Secrets;

#pragma warning disable OPENAI001

namespace Sandbox.Providers;

public static class AzureOpenAI
{
    [AITool]
    static string GetWeather()
    {
        return "Sunny";
    }

    public static async Task RunAsync()
    {
        Secrets.Secrets secrets = SecretsManager.GetSecrets();

        //Create your AgentFactory (using a connection object for more options)
        AzureOpenAIAgentFactory factory = new AzureOpenAIAgentFactory(new AzureOpenAIConnection
        {
            Endpoint = secrets.AzureOpenAiEndpoint,
            ApiKey = secrets.AzureOpenAiKey,
        });

        ChatClientAgent a = factory.Connection.GetClient().GetChatClient(OpenAIChatModels.Gpt41Nano).CreateAIAgent();

        ChatClientAgentRunResponse<Movie[]> response = await a.RunAsync<Movie[]>("Give me the top 3 movies according to IMDB");

        AzureOpenAIAgent agent = factory.CreateAgent(new AgentOptions
        {
            Model = OpenAIChatModels.Gpt41Nano,
            RawToolCallDetails = Console.WriteLine
        });

        ChatClientAgentRunResponse<Movie[]> response2 = await agent.RunAsync<Movie[]>("Give me the top 3 movies according to IMDB");
    }

    private class MovieResult
    {
        public required List<Movie> List { get; set; }
    }

    private class Movie
    {
        public required string Title { get; set; }
    }
}
