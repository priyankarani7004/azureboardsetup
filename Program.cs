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

var modelId = "gpt-4";
var endpoint = "https://soumenopenai.openai.azure.com";
var apiKey = "9834a230b1234782838e45cd6aad4d9f";

// azure url
string orgUrl = "https://dev.azure.com/rohitdecruzgurgaon";
string project = "TestingProject";
string pat = "DXki1K98s5llZhJ7FMPUpqzGtHf6oCycKoze9Qz5jkK1kytpWdHEJQQJ99BDACAAAAAAAAAAAAASAZDO4WxZ";
string requestUri = $"{orgUrl}/{project}/_apis/wit/wiql?api-version=7.1";

string wiqlQuery = "SELECT [System.Id], [System.Title], [System.State] FROM WorkItems";
using (HttpClient client = new HttpClient())
{
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
        Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{pat}")));
        
    HttpContent content = new StringContent(wiqlQuery, System.Text.Encoding.UTF8, "application/json");
    HttpResponseMessage response = await client.PostAsync(requestUri, content);

    if (response.IsSuccessStatusCode)
    {
        string  responseres = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseres);
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
    }
}


// Initialize AutoGen OpenAI client
var openAIClient = new OpenAIClient(apiKey);
var model = "gpt-4";  // Change this depending on your model
// Create a semantic kernel
var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
var kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();

// Start the conversation by asking for the project description
Console.WriteLine("Enter a project description (e.g., 'Develop a mobile app with user authentication, profile, and chat feature.'):");
string projectDescription = Console.ReadLine();

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