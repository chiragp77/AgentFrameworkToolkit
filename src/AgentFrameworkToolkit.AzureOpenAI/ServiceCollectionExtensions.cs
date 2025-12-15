using Azure.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AgentFrameworkToolkit.AzureOpenAI;

/// <summary>
/// Extension Methods for IServiceCollection
/// </summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register an AddAzureOpenAIAgentFactory as a Singleton
    /// </summary>
    /// <param name="services">The IServiceCollection collection</param>
    /// <param name="connection">Connection Details</param>
    /// <returns>The ServiceCollection</returns>
    public static IServiceCollection AddAzureOpenAIAgentFactory(this IServiceCollection services, AzureOpenAIConnection connection)
    {
        return services.AddSingleton(new AzureOpenAIAgentFactory(connection));
    }

    /// <summary>
    /// Register an AddAzureOpenAIAgentFactory as a Singleton
    /// </summary>
    /// <param name="services">The IServiceCollection collection</param>
    /// <param name="endpoint">You Azure OpenAI Endpoint</param>
    /// <param name="apiKey">The API Key</param>
    /// <returns>The ServiceCollection</returns>
    public static IServiceCollection AddAzureOpenAIAgentFactory(this IServiceCollection services, string endpoint, string apiKey)
    {
        return services.AddSingleton(new AzureOpenAIAgentFactory(endpoint, apiKey));
    }

    /// <summary>
    /// Register an AddAzureOpenAIAgentFactory as a Singleton
    /// </summary>
    /// <param name="services">The IServiceCollection collection</param>
    /// <param name="endpoint">You Azure OpenAI Endpoint</param>
    /// <param name="credentials">Your RBAC Credentials</param>
    /// <returns>The ServiceCollection</returns>
    public static IServiceCollection AddAzureOpenAIAgentFactory(this IServiceCollection services, string endpoint, TokenCredential credentials)
    {
        return services.AddSingleton(new AzureOpenAIAgentFactory(endpoint, credentials));
    }

    /// <summary>
    /// Register an AddAzureOpenAIEmbeddingFactory as a Singleton
    /// </summary>
    /// <param name="services">The IServiceCollection collection</param>
    /// <param name="connection">Connection Details</param>
    /// <returns>The ServiceCollection</returns>
    public static IServiceCollection AddAzureOpenAIEmbeddingFactory(this IServiceCollection services, AzureOpenAIConnection connection)
    {
        return services.AddSingleton(new AzureOpenAIEmbeddingFactory(connection));
    }

    /// <summary>
    /// Register an AddAzureOpenAIEmbeddingFactory as a Singleton
    /// </summary>
    /// <param name="services">The IServiceCollection collection</param>
    /// <param name="endpoint">You Azure OpenAI Endpoint</param>
    /// <param name="apiKey">The API Key</param>
    /// <returns>The ServiceCollection</returns>
    public static IServiceCollection AddAzureOpenAIEmbeddingFactory(this IServiceCollection services, string endpoint, string apiKey)
    {
        return services.AddSingleton(new AzureOpenAIEmbeddingFactory(endpoint, apiKey));
    }

    /// <summary>
    /// Register an AddAzureOpenAIEmbeddingFactory as a Singleton
    /// </summary>
    /// <param name="services">The IServiceCollection collection</param>
    /// <param name="endpoint">You Azure OpenAI Endpoint</param>
    /// <param name="credentials">Your RBAC Credentials</param>
    /// <returns>The ServiceCollection</returns>
    public static IServiceCollection AddAzureOpenAIEmbeddingFactory(this IServiceCollection services, string endpoint, TokenCredential credentials)
    {
        return services.AddSingleton(new AzureOpenAIEmbeddingFactory(endpoint, credentials));
    }
}