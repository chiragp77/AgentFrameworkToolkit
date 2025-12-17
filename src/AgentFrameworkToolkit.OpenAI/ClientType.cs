namespace AgentFrameworkToolkit.OpenAI;

/// <summary>
/// The type of OpenAI Client
/// </summary>
public enum ClientType
{
    /// <summary>
    /// The generic ChatClient protocol
    /// </summary>
    ChatClient,

    /// <summary>
    /// The more modern Responses API
    /// </summary>
    ResponsesApi
}