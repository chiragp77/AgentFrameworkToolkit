using JetBrains.Annotations;
using Microsoft.Extensions.AI;

namespace AgentFrameworkToolkit.OpenAI;

/// <summary>
/// Factory for creating OpenAI EmbeddingGenerator
/// </summary>
[PublicAPI]
public class OpenAIEmbeddingFactory
{
    /// <summary>
    /// Connection
    /// </summary>
    public OpenAIConnection Connection { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiKey">Your OpenAI API Key (if you need a more advanced connection use the constructor overload)</param>
    public OpenAIEmbeddingFactory(string apiKey)
    {
        Connection = new OpenAIConnection
        {
            ApiKey = apiKey,
        };
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="connection">Connection Details</param>
    public OpenAIEmbeddingFactory(OpenAIConnection connection)
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
