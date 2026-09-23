using JetBrains.Annotations;

namespace AgentFrameworkToolkit.Cerebras;

/// <summary>
/// A List of Cerebras Production Models
/// </summary>
[PublicAPI]
public static class CerebrasChatModels
{
    /// <summary>
    /// Qwen 3.8 27B
    /// </summary>
    public const string Qwen3827B = "qwen-3.8-27b";

    /// <summary>
    /// Llama 3.1 8B
    /// </summary>
    public const string Llama318B = "llama3.1-8b";

    /// <summary>
    /// GPT OSS 120B
    /// </summary>
    public const string GptOss120B = "gpt-oss-120b";
}
