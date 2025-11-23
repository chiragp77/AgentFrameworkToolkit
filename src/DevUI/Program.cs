using AgentFrameworkToolkit.Anthropic;
using AgentFrameworkToolkit.AzureOpenAI;
using AgentFrameworkToolkit.Google;
using AgentFrameworkToolkit.Mistral;
using AgentFrameworkToolkit.OpenAI;
using AgentFrameworkToolkit.OpenRouter;
using AgentFrameworkToolkit.XAI;
using GenerativeAI;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Mistral.SDK;
using ServiceDefaults;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

string? azureOpenAiEndpoint = builder.Configuration[SecretKeys.AzureOpenAIEndpoint];
string? azureOpenAiApiKey = builder.Configuration[SecretKeys.AzureOpenAIApiKey];
string? openAIApiKey = builder.Configuration[SecretKeys.OpenAIApiKey];
string? mistralApiKey = builder.Configuration[SecretKeys.MistralApiKey];
string? googleApiKey = builder.Configuration[SecretKeys.GoogleApiKey];
string? anthropicApiKey = builder.Configuration[SecretKeys.AnthropicApiKey];
string? xAIApiKey = builder.Configuration[SecretKeys.XAIApiKey];
string? openRouterApiKey = builder.Configuration[SecretKeys.OpenRouterApiKey];

if (HasValidValues(azureOpenAiEndpoint, azureOpenAiApiKey))
{
    const string agentName = "Azure OpenAI Agent";
    builder.AddAIAgent(agentName, (_, _) => new AzureOpenAIAgentFactory(azureOpenAiEndpoint!, azureOpenAiApiKey!).CreateAgent(deploymentModelName: OpenAIChatModels.Gpt41Mini, name: agentName));
}

if (HasValidValues(openAIApiKey))
{
    const string agentName = "OpenAI Agent";
    builder.AddAIAgent(agentName, (_, _) => new OpenAIAgentFactory(openAIApiKey!).CreateAgent(model: OpenAIChatModels.Gpt41Mini, name: agentName));
}

if (HasValidValues(googleApiKey))
{
    const string agentName = "Google Agent";
    builder.AddAIAgent(agentName, (_, _) => new GoogleAgentFactory(googleApiKey!).CreateAgent(model: GoogleChatModels.Gemini25Flash, name: agentName));
}

if (HasValidValues(mistralApiKey))
{
    const string agentName = "Mistral Agent";
    builder.AddAIAgent(agentName, (_, _) => new MistralAgentFactory(mistralApiKey!).CreateAgent(model: MistalChatModels.MistralSmall, name: agentName));
}

if (HasValidValues(anthropicApiKey))
{
    const string agentName = "Anthropic Agent";
    builder.AddAIAgent(agentName, (_, _) => new AnthropicAgentFactory(anthropicApiKey!).CreateAgent(model: AnthropicChatModels.ClaudeHaiku45, maxTokenCount: 1000, name: agentName));
}

if (HasValidValues(xAIApiKey))
{
    const string agentName = "XAI Agent";
    builder.AddAIAgent(agentName, (_, _) => new XAIAgentFactory(xAIApiKey!).CreateAgent(model: XAIChatModels.Grok41FastNonReasoning, name: agentName));
}

if (HasValidValues(openRouterApiKey))
{
    const string agentName = "OpenRouter Agent";
    builder.AddAIAgent(agentName, (_, _) => new OpenRouterAgentFactory(openRouterApiKey!).CreateAgent(model: OpenRouterChatModels.OpenAI.Gpt41Mini, name: agentName));
}

// Register Services needed to run DevUI
builder.Services.AddOpenAIResponses();
builder.Services.AddOpenAIConversations();

WebApplication app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    //Needed for DevUI to function 
    app.MapOpenAIResponses();
    app.MapOpenAIConversations();
    app.MapDevUI();
}

app.UseHttpsRedirection();

app.Run();
return;

bool HasValidValues(params string?[] values)
{
    foreach (string? value in values)
    {
        if (value?.ToUpperInvariant() is null or "" or "-" or "NONE")
        {
            return false;
        }
    }

    return true;
}