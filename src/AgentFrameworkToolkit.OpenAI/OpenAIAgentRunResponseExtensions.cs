using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Containers;
using OpenAI.Responses;
using System.ClientModel;

#pragma warning disable OPENAI001

namespace AgentFrameworkToolkit.OpenAI;

/// <summary>
/// Various handy Extensions for an AgentRunResponse that are OpenAI Specific
/// </summary>
public static class OpenAIAgentRunResponseExtensions
{
    /// <summary>
    /// Get all ContainerFileCitationMessageAnnotations from a 
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static IList<ContainerFileCitationMessageAnnotation>? GetContainerFileCitationMessageAnnotations(this AgentRunResponse response)
    {
        List<ContainerFileCitationMessageAnnotation> result = [];
        foreach (ChatMessage message in response.Messages)
        {
            foreach (AIContent content in message.Contents)
            {
                if (content.Annotations == null)
                {
                    continue;
                }

                foreach (AIAnnotation annotation in content.Annotations)
                {
                    if (annotation.RawRepresentation is ContainerFileCitationMessageAnnotation containerFileCitation)
                    {
                        result.Add(containerFileCitation);
                    }
                }
            }
        }

        return result;
    }
}
