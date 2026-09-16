using Microsoft.Extensions.AI;
using System.ClientModel.Primitives;

namespace AgentFrameworkToolkit.OpenAI;

internal static class AzureImageGenerationSupport
{
    private const string DeploymentHeaderName = "x-ms-oai-image-generation-deployment";

    internal static void Configure(
        ClientPipelineOptions clientOptions,
        AgentOptions agentOptions,
        ClientType defaultClientType)
    {
        if ((agentOptions.ClientType ?? defaultClientType) != ClientType.ResponsesApi)
        {
            return;
        }

        string? deployment = agentOptions.Tools?
            .OfType<HostedImageGenerationTool>()
            .Select(tool => tool.Options?.ModelId)
            .FirstOrDefault(modelId => !string.IsNullOrWhiteSpace(modelId));

        if (deployment != null)
        {
            clientOptions.AddPolicy(new ImageGenerationDeploymentPolicy(deployment), PipelinePosition.PerCall);
        }
    }

    private sealed class ImageGenerationDeploymentPolicy(string deployment) : PipelinePolicy
    {
        public override void Process(
            PipelineMessage message,
            IReadOnlyList<PipelinePolicy> pipeline,
            int currentIndex)
        {
            message.Request.Headers.Set(DeploymentHeaderName, deployment);
            ProcessNext(message, pipeline, currentIndex);
        }

        public override ValueTask ProcessAsync(
            PipelineMessage message,
            IReadOnlyList<PipelinePolicy> pipeline,
            int currentIndex)
        {
            message.Request.Headers.Set(DeploymentHeaderName, deployment);
            return ProcessNextAsync(message, pipeline, currentIndex);
        }
    }
}
