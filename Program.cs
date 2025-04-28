using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using AutoGen;
using AutoGen.Core;
using AutoGen.OpenAI;
using OpenAI;
using System.ClientModel;
using AutoGen.OpenAI.Extension;
using Azure.AI.OpenAI;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Semantickernal;
using System.Text.Json;

var modelId = "gpt-4";
var endpoint = "https://soumenopenai.openai.azure.com";
var apiKey = "9834a230b1234782838e45cd6aad4d9f";

// azure url
string orgUrl = "https://dev.azure.com/rohitdecruzgurgaon/TestingProject/_apis/wit/workitems?ids=1&api-version=7.1";
string project = "TestingProject";
string pat = "DXki1K98s5llZhJ7FMPUpqzGtHf6oCycKoze9Qz5jkK1kytpWdHEJQQJ99BDACAAAAAAAAAAAAASAZDO4WxZ";
string requestUri = $"{orgUrl}/{project}/_apis/wit/wiql?api-version=7.1";

var service = new AzureDevOpsService();
var personalAccessToken = "DXki1K98s5llZhJ7FMPUpqzGtHf6oCycKoze9Qz5jkK1kytpWdHEJQQJ99BDACAAAAAAAAAAAAASAZDO4WxZ";

var titles = await service.FetchAllWorkItemsAsync(personalAccessToken);

var openAIClient = new OpenAIClient(apiKey);
var model = "gpt-4";  // Change this depending on your model
// Create a semantic kernel
var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
var kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();

Console.WriteLine("Work Item Titles:");
foreach (var title in titles)
{
    Console.WriteLine(title);

    //Console.WriteLine("Enter a project description (e.g., 'Develop a mobile app with user authentication, profile, and chat feature.'):");
    string projectDescription =title;

    // Add system message for timeline generation
    history.AddSystemMessage("You are a project management assistant. Your task is to generate a weekly timeline for a given project, breaking it down into tasks including design, development, testing, and deployment phases.");

    // Add the project description to the chat history
    history.AddUserMessage($"Project description: {projectDescription}");

    // Call the OpenAI model to generate the timeline
    var result = await chatService.GetChatMessageContentAsync(
        history,
        executionSettings: new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        },
        kernel: kernel
    );

    // Print the generated project timeline
    Console.WriteLine("\n🧠 Estimated Project Timeline:\n");
    Console.WriteLine(result.Content);
}
// Initialize AutoGen OpenAI client

// Start the conversation by asking for the project description
