using AgentFrameworkToolkit.OpenAI;
using JetBrains.Annotations;
using Microsoft.Extensions.AI;

namespace AgentFrameworkToolkit.OpenRouter;

/// <summary>
/// Factory for creating AzureOpenAI EmbeddingGenerator
/// </summary>
[PublicAPI]
public class OpenRouterEmbeddingFactory
{
    private readonly OpenAIEmbeddingFactory _openAIEmbeddingFactory;

    /// <summary>
    /// Connection
    /// </summary>
    public OpenRouterConnection Connection { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiKey">Your OpenRouter API Key (if you need a more advanced connection use the constructor overload)</param>
    public OpenRouterEmbeddingFactory(string apiKey)
    {
        Connection = new OpenRouterConnection
        {
            ApiKey = apiKey,
            Endpoint = OpenRouterConnection.DefaultEndpoint
        };

        _openAIEmbeddingFactory = new OpenAIEmbeddingFactory(Connection);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="connection">Connection Details</param>
    public OpenRouterEmbeddingFactory(OpenRouterConnection connection)
    {
        connection.Endpoint ??= OpenRouterConnection.DefaultEndpoint;
        Connection = connection;
        _openAIEmbeddingFactory = new OpenAIEmbeddingFactory(connection);
    }

    /// <summary>
    /// Create an EmbeddingGenerator
    /// </summary>
    /// <param name="model">The Embedding Model to use</param>
    /// <returns>An Embedding Generator</returns>
    public IEmbeddingGenerator<string, Embedding<float>> GetEmbeddingGenerator(string model)
    {
        return _openAIEmbeddingFactory.GetEmbeddingGenerator(model);
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
        return await _openAIEmbeddingFactory.GenerateAsync(value, model, options, cancellationToken);
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
        return await _openAIEmbeddingFactory.GenerateAsync(values, model, options, cancellationToken);
    }
}
