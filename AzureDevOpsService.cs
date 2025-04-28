using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Semantickernal
{
    public class AzureDevOpsService
    {
        private readonly HttpClient _httpClient;
        private const string WiqlUrl = "https://dev.azure.com/rohitdecruzgurgaon/TestingProject/_apis/wit/wiql?api-version=7.1";
        private const string WorkItemsUrl = "https://dev.azure.com/rohitdecruzgurgaon/TestingProject/_apis/wit/workitems";

        public AzureDevOpsService()
        {
            _httpClient = new HttpClient();
        }

        // Step 1: Fetch all work item IDs using WIQL

        private async Task<string> FetchWorkItemDetailsAsync(int workItemId, string personalAccessToken)
        {
            var url = $"{WorkItemsUrl}/{workItemId}?api-version=7.1";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("fields", out var fields) &&
                    fields.TryGetProperty("System.Title", out var title))
                {
                    return title.GetString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching details for Work Item {workItemId}: {ex.Message}");
            }

            return "Unknown Title"; // Fallback title in case of an error
        }

        // Step 2: Fetch work item details by ID
        public async Task<List<string>> FetchAllWorkItemsAsync(string personalAccessToken)
        {
            var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{personalAccessToken}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var wiqlQuery = new { query = "SELECT [System.Id] FROM WorkItems" };
            var jsonQuery = JsonSerializer.Serialize(wiqlQuery);
            var content = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            var titles = new List<string>();

            try
            {
                var wiqlResponse = await _httpClient.PostAsync(WiqlUrl, content);
                wiqlResponse.EnsureSuccessStatusCode();

                var wiqlResponseBody = await wiqlResponse.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(wiqlResponseBody);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("workItems", out var workItems))
                {
                    foreach (var workItem in workItems.EnumerateArray())
                    {
                        var id = workItem.GetProperty("id").GetInt32();
                        var title = await FetchWorkItemDetailsAsync(id, personalAccessToken);
                        titles.Add(title);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching work items: {ex.Message}");
            }

            return titles;
        }

    }
}
