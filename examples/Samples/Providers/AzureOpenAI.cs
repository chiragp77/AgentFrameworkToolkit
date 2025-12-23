using AgentFrameworkToolkit;
using AgentFrameworkToolkit.AzureOpenAI;
using AgentFrameworkToolkit.OpenAI;
using AgentFrameworkToolkit.Tools;
using AgentFrameworkToolkit.Tools.ModelContextProtocol;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ModelContextProtocol.Client;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Secrets;

#pragma warning disable OPENAI001

namespace Samples.Providers;

public static class AzureOpenAI
{
    [AITool]
    static string GetWeather()
    {
        return "Sunny";
    }

    public static async Task RunAsync()
    {
        string sourceName = "AiSource";
        var tracerProviderBuilder = Sdk.CreateTracerProviderBuilder()
            .AddSource(sourceName)
            .AddConsoleExporter();
        using TracerProvider tracerProvider = tracerProviderBuilder.Build();

        Secrets.Secrets secrets = SecretsManager.GetSecrets();
        AzureOpenAIConnection connection = new()
        {
            Endpoint = secrets.AzureOpenAiEndpoint,
            ApiKey = secrets.AzureOpenAiKey
        };

        AzureOpenAIAgentFactory factory = new(connection);

        AIToolsFactory aiToolsFactory = new();

        await using McpClientTools mcpClientTools = await aiToolsFactory.GetToolsFromRemoteMcpAsync("https://mcp.relewise.com");
        AzureOpenAIAgent agent = factory.CreateAgent(new AgentOptions
        {
            Model = OpenAIChatModels.Gpt41Mini,
            Tools = mcpClientTools.Tools,
            RawToolCallDetails = Console.WriteLine,
            OpenTelemetryMiddleware = new OpenTelemetryMiddleware(sourceName, telemetryAgent => telemetryAgent.EnableSensitiveData = true)
        });

        AgentRunResponse response = await agent.RunAsync("Use the 'search_docs' tool to find out what relewise user-types exist?");
        Console.WriteLine(response);
    }
}
