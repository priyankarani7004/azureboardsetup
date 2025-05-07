// Import packages
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;




 
var modelId = "gpt-4";
var endpoint = "https://soumenopenai.openai.azure.com";
var apiKey = "9834a230b1234782838e45cd6aad4d9f";
ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
HttpClientHandler handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};
HttpClient client = new HttpClient(handler);
// Initialize Semantic Kernel
var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey,httpClient:client);
Kernel kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
// Create plugin and register it
var comparisonPlugin = new ProductComparisonPlugin(chatService);
var productdetailPlugin = new ProductDetailPlugin(chatService);
var sentimantAnalysis = new SentimantAnalysis(chatService);
//plugin register inside the krnal:-
kernel.Plugins.AddFromObject(comparisonPlugin, "ProductComparison");
kernel.Plugins.AddFromObject(productdetailPlugin, "ProductDetailPlugin");
kernel.Plugins.AddFromObject(sentimantAnalysis, "SentimantAnalysis");
// string? url1 = Console.ReadLine();
OpenAIPromptExecutionSettings executionSettings = new()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
    Temperature = 0.7,
    MaxTokens = 100
};
Console.WriteLine("Enter the Product Url1");
string? url1 = Console.ReadLine();

var history = new ChatHistory();history.AddUserMessage($"{url1}");
IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// var agent = new ChatCompletionAgent(chatService, agentOptions);
var result = await chatCompletionService.GetChatMessageContentAsync(
   history,
   executionSettings: executionSettings,
   kernel: kernel);
Console.WriteLine("Assistant > " + result);history.AddAssistantMessage(result.ToString());



 
