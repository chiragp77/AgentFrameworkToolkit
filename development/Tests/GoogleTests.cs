using AgentFrameworkToolkit.Google;
using Microsoft.Extensions.DependencyInjection;
using Secrets;

namespace AgentFrameworkToolkit.Tests;

[Collection("AgentFactoryTests")]
public sealed class GoogleAgentFactoryTests : TestsBase
{
    [Fact]
    public Task AgentFactory_Simple() => SimpleAgentTestsAsync(AgentProvider.Google);

    [Fact]
    public Task AgentFactory_Normal() => NormalAgentTestsAsync(AgentProvider.Google);

    [Fact]
    public Task AgentFactory_OpenTelemetryAndLoggingMiddleware() => OpenTelemetryAndLoggingMiddlewareTestsAsync(AgentProvider.Google);

    [Fact]
    public Task AgentFactory_ToolCall() => ToolCallAgentTestsAsync(AgentProvider.Google);

    [Fact]
    public async Task AgentFactory_DependencyInjection()
    {
        var secrets = SecretsManager.GetSecrets();
        ServiceCollection services = new();
        services.AddGoogleAgentFactory(secrets.GoogleGeminiApiKey);

        ServiceProvider provider = services.BuildServiceProvider();

        var cancellationToken = TestContext.Current.CancellationToken;
        string text = (await provider.GetRequiredService<GoogleAgentFactory>()
            .CreateAgent(GoogleChatModels.Gemini25Flash)
            .RunAsync("Hello", cancellationToken: cancellationToken)).Text;
        Assert.NotEmpty(text);
    }

    [Fact]
    public async Task AgentFactory_DependencyInjection_Connection()
    {
        var secrets = SecretsManager.GetSecrets();
        ServiceCollection services = new();
        services.AddGoogleAgentFactory(new GoogleConnection
        {
            ApiKey = secrets.GoogleGeminiApiKey
        });

        ServiceProvider provider = services.BuildServiceProvider();

        var cancellationToken = TestContext.Current.CancellationToken;
        string text = (await provider.GetRequiredService<GoogleAgentFactory>()
            .CreateAgent(GoogleChatModels.Gemini25Flash)
            .RunAsync("Hello", cancellationToken: cancellationToken)).Text;
        Assert.NotEmpty(text);
    }
}
