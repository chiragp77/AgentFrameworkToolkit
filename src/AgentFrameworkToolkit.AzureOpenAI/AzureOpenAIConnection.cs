using Azure.AI.OpenAI;
using Azure.Core;

namespace AgentFrameworkToolkit.AzureOpenAI;

/// <summary>
/// Represents a connection for Azure OpenAI
/// </summary>
public class AzureOpenAIConnection
{
    /// <summary>
    /// The Endpoint of your Azure OpenAI Resource
    /// </summary>
    public required string Endpoint { get; set; }

    /// <summary>
    /// The API Key (or use Credentials instead for RBAC)
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Credentials for Role-Based Access Control (Example 'DefaultAzureCredential' or 'AzureCliCredential') [or use ApiKey instead]
    /// </summary>
    public TokenCredential? Credentials { get; set; }

    /// <summary>
    /// An Action that allow you to set additional options on the AzureOpenAIClientOptions
    /// </summary>
    public Action<AzureOpenAIClientOptions>? AdditionalAzureOpenAIClientOptions { get; set; }

    /// <summary>
    /// The timeout value of the LLM Call (if not defined the underlying infrastructure's default will be used)
    /// </summary>
    public TimeSpan? NetworkTimeout { get; set; }
}