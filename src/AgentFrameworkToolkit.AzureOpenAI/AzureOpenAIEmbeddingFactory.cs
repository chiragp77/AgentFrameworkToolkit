using Azure.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.AI;

namespace AgentFrameworkToolkit.AzureOpenAI;

/// <summary>
/// Factory for creating AzureOpenAI EmbeddingGenerator
/// </summary>
[PublicAPI]
public class AzureOpenAIEmbeddingFactory
{
    private readonly AzureOpenAIConnection _connection;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="endpoint">Your AzureOpenAI Endpoint (not to be confused with a Microsoft Foundry Endpoint. format: 'https://YourName.openai.azure.com' or 'https://YourName.services.azure.com')</param>
    /// <param name="apiKey">Your AzureOpenAI API Key (if you need a more advanced connection use the constructor overload)</param>
    public AzureOpenAIEmbeddingFactory(string endpoint, string apiKey)
    {
        _connection = new AzureOpenAIConnection
        {
            Endpoint = endpoint,
            ApiKey = apiKey,
        };
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="endpoint">Your AzureOpenAI Endpoint (not to be confused with a Microsoft Foundry Endpoint. format: 'https://YourName.openai.azure.com' or 'https://YourName.services.azure.com')</param>
    /// <param name="credentials">Your RBAC Credentials (if you need a more advanced connection use the constructor overload)</param>
    public AzureOpenAIEmbeddingFactory(string endpoint, TokenCredential credentials)
    {
        _connection = new AzureOpenAIConnection
        {
            Endpoint = endpoint,
            Credentials = credentials
        };
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="connection">Connection Details</param>
    public AzureOpenAIEmbeddingFactory(AzureOpenAIConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Create an EmbeddingGenerator
    /// </summary>
    /// <param name="model">The Embedding Model to use</param>
    /// <returns>An Embedding Generator</returns>
    public IEmbeddingGenerator<string, Embedding<float>> GetEmbeddingGenerator(string model)
    {
        return _connection.GetClient().GetEmbeddingClient(model).AsIEmbeddingGenerator();
    }
}
