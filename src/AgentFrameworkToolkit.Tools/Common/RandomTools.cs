using JetBrains.Annotations;
using Microsoft.Extensions.AI;

namespace AgentFrameworkToolkit.Tools.Common;

/// <summary>
/// Tools for random number generation
/// </summary>
[PublicAPI]
public static class RandomTools
{
    /// <summary>
    /// Get all random tools with their default settings
    /// </summary>
    /// <param name="options">Optional options</param>
    /// <returns>Tools</returns>
    public static IList<AITool> All(GetRandomCommonToolsOptions? options = null)
    {
        GetRandomCommonToolsOptions optionsToUse = options ?? new GetRandomCommonToolsOptions();
        List<AITool> result = [];
        if (optionsToUse.GetRandomInteger)
        {
            result.Add(GetRandomInteger(optionsToUse.GetRandomIntegerOptions));
        }
        if (optionsToUse.GetRandomDouble)
        {
            result.Add(GetRandomDouble(optionsToUse.GetRandomDoubleOptions));
        }
        return result;
    }

    /// <summary>
    /// Get a random integer in range [min, max)
    /// </summary>
    /// <param name="options">Optional options</param>
    /// <param name="toolName">Name of tool</param>
    /// <param name="toolDescription">Description of Tool</param>
    /// <returns>Tool</returns>
    public static AITool GetRandomInteger(GetRandomIntegerOptions? options = null, string? toolName = null, string? toolDescription = null)
    {
        GetRandomIntegerOptions optionsToUse = options ?? new GetRandomIntegerOptions();
        return AIFunctionFactory.Create((int? min = null, int? max = null) =>
        {
            int effectiveMin = min ?? optionsToUse.DefaultMin;
            int effectiveMax = max ?? optionsToUse.DefaultMax;
            if (effectiveMin >= effectiveMax)
            {
                throw new ArgumentException($"Invalid range. min ({effectiveMin}) must be less than max ({effectiveMax}).");
            }

            return Random.Shared.Next(effectiveMin, effectiveMax);
        }, toolName ?? "get_random_integer", toolDescription ?? "Get a random integer in range [min, max)");
    }

    /// <summary>
    /// Get a random double in range [min, max)
    /// </summary>
    /// <param name="options">Optional options</param>
    /// <param name="toolName">Name of tool</param>
    /// <param name="toolDescription">Description of Tool</param>
    /// <returns>Tool</returns>
    public static AITool GetRandomDouble(GetRandomDoubleOptions? options = null, string? toolName = null, string? toolDescription = null)
    {
        GetRandomDoubleOptions optionsToUse = options ?? new GetRandomDoubleOptions();
        return AIFunctionFactory.Create((double? min = null, double? max = null) =>
        {
            double effectiveMin = min ?? optionsToUse.DefaultMin;
            double effectiveMax = max ?? optionsToUse.DefaultMax;
            if (effectiveMin >= effectiveMax)
            {
                throw new ArgumentException($"Invalid range. min ({effectiveMin}) must be less than max ({effectiveMax}).");
            }

            return effectiveMin + (Random.Shared.NextDouble() * (effectiveMax - effectiveMin));
        }, toolName ?? "get_random_double", toolDescription ?? "Get a random double in range [min, max)");
    }
}

/// <summary>
/// Options for random tools
/// </summary>
[PublicAPI]
public class GetRandomCommonToolsOptions
{
    /// <summary>
    /// Include random integer tool (Default: true)
    /// </summary>
    public bool GetRandomInteger { get; set; } = true;

    /// <summary>
    /// Include random double tool (Default: true)
    /// </summary>
    public bool GetRandomDouble { get; set; } = true;

    /// <summary>
    /// Options for random integer tool
    /// </summary>
    public GetRandomIntegerOptions? GetRandomIntegerOptions { get; set; }

    /// <summary>
    /// Options for random double tool
    /// </summary>
    public GetRandomDoubleOptions? GetRandomDoubleOptions { get; set; }
}

/// <summary>
/// Options for random integer tool
/// </summary>
[PublicAPI]
public class GetRandomIntegerOptions
{
    /// <summary>
    /// Default minimum value (inclusive)
    /// </summary>
    public int DefaultMin { get; set; } = 0;

    /// <summary>
    /// Default maximum value (exclusive)
    /// </summary>
    public int DefaultMax { get; set; } = 100;
}

/// <summary>
/// Options for random double tool
/// </summary>
[PublicAPI]
public class GetRandomDoubleOptions
{
    /// <summary>
    /// Default minimum value (inclusive)
    /// </summary>
    public double DefaultMin { get; set; } = 0.0;

    /// <summary>
    /// Default maximum value (exclusive)
    /// </summary>
    public double DefaultMax { get; set; } = 1.0;
}
