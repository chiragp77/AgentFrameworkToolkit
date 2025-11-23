using Microsoft.Extensions.DependencyInjection;

namespace AgentFrameworkToolkit.OpenRouter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenRouterAgentFactory(this IServiceCollection services, OpenRouterConnection connection)
    {
        return services.AddSingleton(new OpenRouterAgentFactory(connection));
    }

    public static IServiceCollection AddOpenRouterAgentFactory(this IServiceCollection services, string apiKey)
    {
        return services.AddSingleton(new OpenRouterAgentFactory(apiKey));
    }
}