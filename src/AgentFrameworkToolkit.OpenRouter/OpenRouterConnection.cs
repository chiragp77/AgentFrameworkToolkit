using OpenAI;

namespace AgentFrameworkToolkit.OpenRouter;

public class OpenRouterConnection
{
    public required string ApiKey { get; set; }

    public Action<OpenAIClientOptions>? AdditionalOpenAIClientOptions { get; set; }
}