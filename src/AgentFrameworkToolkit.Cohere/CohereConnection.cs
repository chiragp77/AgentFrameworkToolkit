using System.Diagnostics.CodeAnalysis;
using AgentFrameworkToolkit.OpenAI;
using JetBrains.Annotations;

namespace AgentFrameworkToolkit.Cohere;

/// <summary>
/// Represents a connection for Cohere
/// </summary>
[PublicAPI]
public class CohereConnection : OpenAIConnection
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CohereConnection()
    {
        //Empty
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiKey">The API Key to be used</param>
    [SetsRequiredMembers]
    public CohereConnection(string apiKey) : base(apiKey)
    {
        //Empty
    }

    /// <summary>
    /// The Default Cohere Endpoint
    /// </summary>
    public const string DefaultEndpoint = "https://api.cohere.ai/compatibility/v1";
}