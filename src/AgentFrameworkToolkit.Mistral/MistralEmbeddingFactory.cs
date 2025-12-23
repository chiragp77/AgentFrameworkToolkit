using JetBrains.Annotations;
using Microsoft.Extensions.AI;
using Mistral.SDK;

namespace AgentFrameworkToolkit.Mistral;

/// <summary>
/// Factory for creating OpenAI EmbeddingGenerator
/// </summary>
[PublicAPI]
public class MistralEmbeddingFactory
{
    private readonly MistralConnection _connection;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiKey">Your Mistral API Key (if you need a more advanced connection use the constructor overload)</param>
    public MistralEmbeddingFactory(string apiKey)
    {
        _connection = new MistralConnection
        {
            ApiKey = apiKey
        };
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="connection">Connection Details</param>
    public MistralEmbeddingFactory(MistralConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Create an EmbeddingGenerator (Note: model-ID need to be provided when generating the Vector and not to this method)
    /// </summary>
    /// <returns>An Embedding Generator</returns>
    public IEmbeddingGenerator<string, Embedding<float>> GetEmbeddingGenerator()
    {
        MistralClient client = _connection.GetClient();
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = client.Embeddings;
        return embeddingGenerator;
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
        return await GenerateAsync(value, new EmbeddingGenerationOptions
        {
            ModelId = model
        }, cancellationToken);
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
        return await GenerateAsync(values, new EmbeddingGenerationOptions
        {
            ModelId = model
        }, cancellationToken);
    }

    /// <summary>
    /// Generate an embedding
    /// </summary>
    /// <param name="value">String to embed</param>
    /// <param name="options">Options for the Embedding</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>The Embedding</returns>
    public async Task<Embedding<float>> GenerateAsync(string value, EmbeddingGenerationOptions options, CancellationToken cancellationToken = default)
    {
        IEmbeddingGenerator<string, Embedding<float>> generator = GetEmbeddingGenerator();
        return await generator.GenerateAsync(value, options, cancellationToken);
    }


    /// <summary>
    /// Generate an embedding
    /// </summary>
    /// <param name="values">Strings to embed</param>
    /// <param name="options">Options for the Embedding</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>The Embeddings</returns>
    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(IEnumerable<string> values, EmbeddingGenerationOptions options, CancellationToken cancellationToken = default)
    {
        IEmbeddingGenerator<string, Embedding<float>> generator = GetEmbeddingGenerator();
        return await generator.GenerateAsync(values, options, cancellationToken);
    }
}
