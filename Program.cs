// Import packages
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
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
kernel.Plugins.AddFromObject(comparisonPlugin, "ProductComparison");

var planner = new SemanticPlanner(comparisonPlugin);

string goal = "Compare www.google.com/iPhone 14 and www.google.com/Samsung S23";

// Execute plan
string result = await planner.CreatePlanAsync(goal);

// Output result
Console.WriteLine("\n🧠 Goal: " + goal);
Console.WriteLine("📊 Result:\n" + result);
