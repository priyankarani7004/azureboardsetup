// Import packages
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var modelId = "gpt-4";
var endpoint = "https://soumenopenai.openai.azure.com";
var apiKey = "9834a230b1234782838e45cd6aad4d9f";

// Initialize Semantic Kernel
var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
Kernel kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();

var comparisonPlugin = new ProductComparisonPlugin(chatService);
string comparisonResult = await comparisonPlugin.CompareProductsAsync("https://www.example.com/product1", "https://www.example.com/product2");

Console.WriteLine("\n🛍️ Product Comparison:\n" + comparisonResult);

//var history = new ChatHistory();

//// URLs to compare
//string url1 = "https://www.xerve.in/nokia";
//string url2 = "https://www.xerve.in/nokia";

//string html1 = await GetHtmlAsync(url1);
//string html2 = await GetHtmlAsync(url2);

//// Add instruction to the chat history
//history.AddSystemMessage("You are a product comparison assistant. Compare two product pages in detail including specs, price, brand, and reviews.");

//// Add both HTMLs
//history.AddUserMessage($"Product page from URL 1:\n{html1}");
//history.AddUserMessage($"Product page from URL 2:\n{html2}");

//var result = await chatService.GetChatMessageContentAsync(
//    history,
//    executionSettings: new OpenAIPromptExecutionSettings
//    {
//        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
//    },
//    kernel: kernel
//);

//Console.WriteLine("\n🧠 Assistant's Comparison:\n");
//Console.WriteLine(result.Content);


//static async Task<string> GetHtmlAsync(string url)
//{
//    using HttpClient client = new();
//    return await client.GetStringAsync(url);
//}