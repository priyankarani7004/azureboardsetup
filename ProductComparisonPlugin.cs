using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public class ProductComparisonPlugin
{
    private readonly IChatCompletionService _chatService;

    public ProductComparisonPlugin(IChatCompletionService chatService)
    {
        _chatService = chatService;
    }

    public async Task<string> CompareProductsAsync(string url1, string url2)
    {
        string html1 = await GetHtmlAsync(url1);
        string html2 = await GetHtmlAsync(url2);

        var history = new ChatHistory();
        history.AddSystemMessage("You are a product comparison assistant. Compare two product pages including specs, price, brand, and reviews.");
        history.AddUserMessage($"Product page from URL 1:\n{html1}");
        history.AddUserMessage($"Product page from URL 2:\n{html2}");

        var result = await _chatService.GetChatMessageContentAsync(history);
        return result.Content;
    }

    private static async Task<string> GetHtmlAsync(string url)
    {
        using HttpClient client = new();
        return await client.GetStringAsync(url);
    }
}
