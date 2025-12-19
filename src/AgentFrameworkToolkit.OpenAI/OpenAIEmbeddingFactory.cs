using JetBrains.Annotations;
using Microsoft.Extensions.AI;

namespace AgentFrameworkToolkit.OpenAI;

/// <summary>
/// Factory for creating OpenAI EmbeddingGenerator
/// </summary>
[PublicAPI]
public class OpenAIEmbeddingFactory
{
    private readonly OpenAIConnection _connection;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiKey">Your OpenAI API Key (if you need a more advanced connection use the constructor overload)</param>
    public OpenAIEmbeddingFactory(string apiKey)
    {
        _connection = new OpenAIConnection
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
