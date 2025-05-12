// Import packages
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Semantickernal;

var modelId = "gpt-4";
var endpoint = "https://soumenopenai.openai.azure.com";
var apiKey = "9834a230b1234782838e45cd6aad4d9f";

// Initialize Semantic Kernel
var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
Kernel kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
// Create plugin and register it
var comparisonPlugin = new ProductComparisonPlugin(chatService);


var comparerKernel = kernel.Clone();
var helperKernel = kernel.Clone();

// Add plugin functions
var comparerFunction = comparerKernel.CreateFunctionFromMethod(CustomProductPlugin.CompareProducts);
comparerKernel.ImportPluginFromFunctions("product", new[] { comparerFunction });

var helperFunction = helperKernel.CreateFunctionFromMethod(CustomProductPlugin.SummarizeComparison);
helperKernel.ImportPluginFromFunctions("helper", new[] { helperFunction });


var comparerAgent = new ChatCompletionAgent
{
    Name = "ProductComparer",
    Instructions = "You are a tech product comparison expert. Use plugin functions to compare and list features.",
    Kernel = comparerKernel,
    Arguments = new KernelArguments(new OpenAIPromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()  // let model choose best function
    })
};

var helperAgent = new ChatCompletionAgent
{
    Name = "HelperAgent",
    Instructions = "Summarize the result using the 'SummarizeComparison' function.",
    Kernel = kernel,
    Arguments = new KernelArguments
            {
                { "input", "Comparison results" }
            }
};

// Create group chat
var chat = new AgentGroupChat(comparerAgent, helperAgent);

// Start conversation
chat.AddChatMessage(new ChatMessageContent(AuthorRole.User, "Compare iPhone 15 and Galaxy S23."));

Console.WriteLine("🤖 Group Chat with Plugin Functions:\n");

await foreach (var message in chat.InvokeAsync())
{
    Console.WriteLine($"{message.Content}");
}