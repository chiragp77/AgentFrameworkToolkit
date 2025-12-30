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
    /// <summary>
    /// Connection
    /// </summary>
    public AzureOpenAIConnection Connection { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="endpoint">Your AzureOpenAI Endpoint (not to be confused with a Microsoft Foundry Endpoint. format: 'https://YourName.openai.azure.com' or 'https://YourName.services.azure.com')</param>
    /// <param name="apiKey">Your AzureOpenAI API Key (if you need a more advanced connection use the constructor overload)</param>
    public AzureOpenAIEmbeddingFactory(string endpoint, string apiKey)
    {
        Connection = new AzureOpenAIConnection
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
        Connection = new AzureOpenAIConnection
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
        Connection = connection;
    }

    /// <summary>
    /// Create an EmbeddingGenerator
    /// </summary>
    /// <param name="model">The Embedding Model to use</param>
    /// <returns>An Embedding Generator</returns>
    public IEmbeddingGenerator<string, Embedding<float>> GetEmbeddingGenerator(string model)
    {
        return Connection.GetClient().GetEmbeddingClient(model).AsIEmbeddingGenerator();
    }


    /// <summary>
    /// Generate an embedding
    /// </summary>
    /// <param name="value">String to embed</param>
    /// <param name="model">Embedding model to use</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>The Embedding</returns>
    public async Task<Embedding<float>> GenerateAsync(string value, string model, CancellationToken cancellationToken = default)
    {
        return await GenerateAsync(value, model, null, cancellationToken);
    }

    /// <summary>
    /// Generate an embedding
    /// </summary>
    /// <param name="values">Strings to embed</param>
    /// <param name="model">Embedding model to use</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>The Embeddings</returns>
    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(IEnumerable<string> values, string model, CancellationToken cancellationToken = default)
    {
        return await GenerateAsync(values, model, null, cancellationToken);
    }

    /// <summary>
    /// Generate an embedding
    /// </summary>
    /// <param name="value">String to embed</param>
    /// <param name="model">Model to use for embedding</param>
    /// <param name="options">Options for the Embedding</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>The Embedding</returns>
    public async Task<Embedding<float>> GenerateAsync(string value, string model, EmbeddingGenerationOptions? options, CancellationToken cancellationToken = default)
    {
        IEmbeddingGenerator<string, Embedding<float>> generator = GetEmbeddingGenerator(model);
        return await generator.GenerateAsync(value, options, cancellationToken);
    }


    /// <summary>
    /// Generate an embedding
    /// </summary>
    /// <param name="values">Strings to embed</param>
    /// <param name="model">Model to use for embedding</param>
    /// <param name="options">Options for the Embedding</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>The Embeddings</returns>
    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(IEnumerable<string> values, string model, EmbeddingGenerationOptions? options, CancellationToken cancellationToken = default)
    {
        IEmbeddingGenerator<string, Embedding<float>> generator = GetEmbeddingGenerator(model);
        return await generator.GenerateAsync(values, options, cancellationToken);
    }
}
