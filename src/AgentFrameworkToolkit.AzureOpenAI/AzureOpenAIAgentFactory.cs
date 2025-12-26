using AgentFrameworkToolkit.OpenAI;
using Azure.Core;
using JetBrains.Annotations;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;

#pragma warning disable OPENAI001

namespace AgentFrameworkToolkit.AzureOpenAI;

/// <summary>
/// Factory for creating AzureOpenAI Agents
/// </summary>
[PublicAPI]
public class AzureOpenAIAgentFactory
{
    private readonly AzureOpenAIConnection _connection;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="endpoint">Your AzureOpenAI Endpoint (not to be confused with a Microsoft Foundry Endpoint. format: 'https://YourName.openai.azure.com' or 'https://YourName.services.azure.com')</param>
    /// <param name="apiKey">Your AzureOpenAI API Key (if you need a more advanced connection use the constructor overload)</param>
    public AzureOpenAIAgentFactory(string endpoint, string apiKey)
    {
        _connection = new AzureOpenAIConnection
        {
            Endpoint = endpoint,
            ApiKey = apiKey
        };
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="endpoint">Your AzureOpenAI Endpoint (not to be confused with a Microsoft Foundry Endpoint. format: 'https://YourName.openai.azure.com' or 'https://YourName.services.azure.com')</param>
    /// <param name="credentials">Your RBAC Credentials (if you need a more advanced connection use the constructor overload)</param>
    public AzureOpenAIAgentFactory(string endpoint, TokenCredential credentials)
    {
        _connection = new AzureOpenAIConnection
        {
            Endpoint = endpoint,
            Credentials = credentials
        };
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="connection">Connection Details</param>
    public AzureOpenAIAgentFactory(AzureOpenAIConnection connection)
    {
        _connection = connection;
    }


    /// <summary>
    /// Create a simple Agent with default settings (For more advanced agents use the options overloads)
    /// </summary>
    /// <param name="model">Name of the model to use</param>
    /// <param name="instructions">Instructions for the Agent to follow (aka Developer Message)</param>
    /// <param name="name">Name of the Agent</param>
    /// <param name="tools">Tools for the Agent</param>
    /// <returns>An Agent</returns>
    public AzureOpenAIAgent CreateAgent(string model, string? instructions = null, string? name = null, IList<AITool>? tools = null)
    {
        return CreateAgent(new AgentOptions
        {
            Model = model,
            Name = name,
            Instructions = instructions,
            Tools = tools
        });
    }

    /// <summary>
    /// Create a new Agent
    /// </summary>
    /// <param name="options">Options for the agent</param>
    /// <returns>The Agent</returns>
    public AzureOpenAIAgent CreateAgent(AgentOptions options)
    {
        OpenAIClient client = _connection.GetClient(options.RawHttpCallDetails);

        ChatClientAgentOptions chatClientAgentOptions = CreateChatClientAgentOptions(options);

        ChatClientAgent innerAgent;
        switch (options.ClientType)
        {
            case ClientType.ChatClient:
                innerAgent = client
                    .GetChatClient(options.Model)
                    .CreateAIAgent(chatClientAgentOptions, options.ClientFactory, options.LoggerFactory, options.Services);
                break;
            case ClientType.ResponsesApi:
                innerAgent = client
                    .GetResponsesClient(options.Model)
                    .CreateAIAgent(chatClientAgentOptions, options.ClientFactory, options.LoggerFactory, options.Services);
                break;
            case null:
                innerAgent = _connection.DefaultClientType switch
                {
                    ClientType.ChatClient => client
                        .GetChatClient(options.Model)
                        .CreateAIAgent(chatClientAgentOptions, options.ClientFactory, options.LoggerFactory, options.Services),
                    ClientType.ResponsesApi => client
                        .GetResponsesClient(options.Model)
                        .CreateAIAgent(chatClientAgentOptions, options.ClientFactory, options.LoggerFactory, options.Services),
                    _ => throw new ArgumentOutOfRangeException()
                };
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (options.RawToolCallDetails != null || options.ToolCallingMiddleware != null || options.OpenTelemetryMiddleware != null || options.LoggingMiddleware != null)
        {
            return new AzureOpenAIAgent(options.ApplyMiddleware(innerAgent));
        }

        return new AzureOpenAIAgent(innerAgent);
    }

    private ChatClientAgentOptions CreateChatClientAgentOptions(AgentOptions options)
    {
        bool anyOptionsSet = false;
        ChatOptions chatOptions = new();
        if (options.Tools != null)
        {
            anyOptionsSet = true;
            chatOptions.Tools = options.Tools;
        }

        if (options.MaxOutputTokens.HasValue)
        {
            anyOptionsSet = true;
            chatOptions.MaxOutputTokens = options.MaxOutputTokens.Value;
        }

        if (options.Temperature != null && !OpenAIChatModels.ReasoningModels.Contains(options.Model))
        {
            anyOptionsSet = true;
            chatOptions.Temperature = options.Temperature;
        }

        if (!string.IsNullOrWhiteSpace(options.Instructions))
        {
            anyOptionsSet = true;
            chatOptions.Instructions = options.Instructions;
        }

        string? reasoningEffortAsString = null;
        switch (options.ReasoningEffort)
        {
            case OpenAIReasoningEffort.None:
                reasoningEffortAsString = "none";
                break;
            case OpenAIReasoningEffort.Minimal:
                reasoningEffortAsString = "minimal";
                break;
            case OpenAIReasoningEffort.Low:
                reasoningEffortAsString = "low";
                break;
            case OpenAIReasoningEffort.Medium:
                reasoningEffortAsString = "medium";
                break;
            case OpenAIReasoningEffort.High:
                reasoningEffortAsString = "high";
                break;
            case OpenAIReasoningEffort.ExtraHigh:
                reasoningEffortAsString = "xhigh";
                break;
        }

        if (!string.IsNullOrWhiteSpace(reasoningEffortAsString) && !OpenAIChatModels.NonReasoningModels.Contains(options.Model))
        {
            anyOptionsSet = true;

            switch (options.ClientType)
            {
                case ClientType.ChatClient:
                    chatOptions = chatOptions.WithOpenAIChatClientReasoning(new ChatReasoningEffortLevel(reasoningEffortAsString));
                    break;
                case ClientType.ResponsesApi:
                    switch (options.ReasoningSummaryVerbosity)
                    {
                        case OpenAIReasoningSummaryVerbosity.Auto:
                            chatOptions = chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString), ResponseReasoningSummaryVerbosity.Auto);
                            break;
                        case OpenAIReasoningSummaryVerbosity.Concise:
                            chatOptions = chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString), ResponseReasoningSummaryVerbosity.Concise);
                            break;
                        case OpenAIReasoningSummaryVerbosity.Detailed:
                            chatOptions = chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString), ResponseReasoningSummaryVerbosity.Detailed);
                            break;
                        case null:
                            chatOptions = chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString));
                            break;
                    }

                    break;
                case null:
                    chatOptions = _connection.DefaultClientType switch
                    {
                        ClientType.ChatClient => chatOptions.WithOpenAIChatClientReasoning(new ChatReasoningEffortLevel(reasoningEffortAsString)),
                        ClientType.ResponsesApi => options.ReasoningSummaryVerbosity switch
                        {
                            OpenAIReasoningSummaryVerbosity.Auto => chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString), ResponseReasoningSummaryVerbosity.Auto),
                            OpenAIReasoningSummaryVerbosity.Concise => chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString), ResponseReasoningSummaryVerbosity.Concise),
                            OpenAIReasoningSummaryVerbosity.Detailed => chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString), ResponseReasoningSummaryVerbosity.Detailed),
                            null => chatOptions.WithOpenAIResponsesApiReasoning(new ResponseReasoningEffortLevel(reasoningEffortAsString)),
                            _ => chatOptions
                        },
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        ChatClientAgentOptions chatClientAgentOptions = new()
        {
            Name = options.Name,
            Description = options.Description,
            Id = options.Id,
            AIContextProviderFactory = options.AIContextProviderFactory,
            ChatMessageStoreFactory = options.ChatMessageStoreFactory,
        };
        if (anyOptionsSet)
        {
            chatClientAgentOptions.ChatOptions = chatOptions;
        }

        options.AdditionalChatClientAgentOptions?.Invoke(chatClientAgentOptions);

        return chatClientAgentOptions;
    }
}
