using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace AgentSkillsDotNet;

/// <summary>
/// Extension Methods for IServiceCollection
/// </summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register an AgentSkillsFactory as a Singleton
    /// </summary>
    /// <param name="services">The IServiceCollection collection</param>
    /// <returns>The ServiceCollection</returns>
    public static IServiceCollection AddAgentSkillsFactory(this IServiceCollection services)
    {
        return services.AddSingleton(new AgentSkillsFactory());
    }
}
