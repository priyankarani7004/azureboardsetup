using Microsoft.SemanticKernel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semantickernal
{
    public class SemanticPlanner
    {
        private readonly ProductComparisonPlugin _productComparisonPlugin;
        public SemanticPlanner(ProductComparisonPlugin productComparisonPlugin)
        {
            _productComparisonPlugin = productComparisonPlugin;
        }

        public async Task<string> CreatePlanAsync(string goal)
        {
            // Basic goal parsing using regex or keyword matching
            if (goal.Contains("compare", StringComparison.OrdinalIgnoreCase) && goal.Contains("product", StringComparison.OrdinalIgnoreCase))
            {
                // Example: "Compare iPhone 14 and Samsung S23"
                var match = Regex.Match(goal, @"compare\s+(.*?)\s+and\s+(.*)", RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count >= 3)
                {
                    string product1 = match.Groups[1].Value.Trim();
                    string product2 = match.Groups[2].Value.Trim();
                    return await _productComparisonPlugin.CompareProductsAsync(product1, product2);
                }
                return "⚠️ Couldn't extract products for comparison.";
            }
            return null;
            
            }
        
        }
    }