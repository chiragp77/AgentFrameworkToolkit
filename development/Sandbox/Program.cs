using Sandbox.Providers;

Console.Clear();

//await Sandbox.Providers.Anthropic.RunAsync();
//await Sandbox.Providers.OpenAI.RunAsync();
//await Sandbox.Providers.GitHub.RunAsync();
//await Sandbox.Providers.Cohere.RunAsync();
await AzureOpenAI.RunAsync();
//await Sandbox.Providers.Mistral.RunAsync();
//await Sandbox.Providers.Google.RunAsync();
//await Sandbox.Providers.XAI.RunAsync();
//await Sandbox.Providers.OpenRouter.RunAsync();

Console.WriteLine("Done");
