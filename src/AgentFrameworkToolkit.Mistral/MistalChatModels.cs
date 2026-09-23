using JetBrains.Annotations;

namespace AgentFrameworkToolkit.Mistral;

/// <summary>
/// The various Mistral Models (if a model is not on the list it might still work; these are just the most common models)
/// </summary>
[PublicAPI]
public class MistalChatModels
{
    /// <summary>
    /// Mistral Medium 3.5
    /// </summary>
    public const string MistralMedium35 = "mistral-medium-3-5";

    /// <summary>
    /// Mistral Small 4
    /// </summary>
    public const string MistralSmall4 = "mistral-small-2603";

    /// <summary>
    /// Mistral Large 3
    /// </summary>
    public const string MistralLarge3 = "mistral-large-2512";

    /// <summary>
    /// Ministral 3 14B
    /// </summary>
    public const string Ministral314B = "ministral-14b-2512";

    /// <summary>
    /// Ministral 3 8B
    /// </summary>
    public const string Ministral38B = "ministral-8b-2512";

    /// <summary>
    /// Ministral 3 3B
    /// </summary>
    public const string Ministral33B = "ministral-3b-2512";

    /// <summary>
    /// Mistral (Small)
    /// </summary>
    public const string MistralSmall = "mistral-small-latest";

    /// <summary>
    /// Mistral (Medium)
    /// </summary>
    public const string MistralMedium = "mistral-medium-latest";

    /// <summary>
    /// Mistral (Large)
    /// </summary>
    public const string MistralLarge = "mistral-large-latest";
}
