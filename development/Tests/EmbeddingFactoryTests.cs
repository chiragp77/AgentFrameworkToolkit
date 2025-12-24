using AgentFrameworkToolkit.AzureOpenAI;
using AgentFrameworkToolkit.Mistral;
using AgentFrameworkToolkit.OpenAI;
using AgentFrameworkToolkit.OpenRouter;
using Microsoft.Extensions.AI;
using Mistral.SDK;
using Secrets;

namespace AgentFrameworkToolkit.Tests;

public class EmbeddingFactoryTests
{
    [Fact]
    public async Task AzureOpenAIEmbeddingTestsAsync()
    {
        Secrets.Secrets secrets = SecretsManager.GetSecrets();
        AzureOpenAIEmbeddingFactory factory = new(secrets.AzureOpenAiEndpoint, secrets.AzureOpenAiKey);
        IEmbeddingGenerator<string, Embedding<float>> generator = factory.GetEmbeddingGenerator("text-embedding-3-small");
        Embedding<float> embedding = await generator.GenerateAsync("Hello", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(1536, embedding.Dimensions);
    }

    [Fact]
    public async Task OpenAIEmbeddingTestsAsync()
    {
        Secrets.Secrets secrets = SecretsManager.GetSecrets();
        OpenAIEmbeddingFactory factory = new(secrets.OpenAiApiKey);
        IEmbeddingGenerator<string, Embedding<float>> generator = factory.GetEmbeddingGenerator("text-embedding-3-small");
        Embedding<float> embedding = await generator.GenerateAsync("Hello", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(1536, embedding.Dimensions);
    }

    [Fact]
    public async Task MistralEmbeddingTestsAsync()
    {
        Secrets.Secrets secrets = SecretsManager.GetSecrets();
        MistralEmbeddingFactory factory = new(secrets.MistralApiKey);
        IEmbeddingGenerator<string, Embedding<float>> generator = factory.GetEmbeddingGenerator();
        Embedding<float> embedding = await generator.GenerateAsync("Hello", cancellationToken: TestContext.Current.CancellationToken,
            options: new EmbeddingGenerationOptions
            {
                ModelId = ModelDefinitions.MistralEmbed
            });
        Assert.Equal(1024, embedding.Dimensions);
    }

    [Fact]
    public async Task OpenRouterEmbeddingTestsAsync()
    {
        Secrets.Secrets secrets = SecretsManager.GetSecrets();
        OpenRouterEmbeddingFactory factory = new(secrets.OpenRouterApiKey);
        IEmbeddingGenerator<string, Embedding<float>> generator = factory.GetEmbeddingGenerator("openai/text-embedding-3-small");
        Embedding<float> embedding = await generator.GenerateAsync("Hello", cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(1536, embedding.Dimensions);
    }
}
