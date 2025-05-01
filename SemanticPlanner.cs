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

        public async Task<string> CreatePlanAsync(string goal,string url1,string url2)
        {
            // Basic goal parsing using regex or keyword matching
            if (goal=="Compare")
            {
                // Example: "Compare iPhone 14 and Samsung S23"
                
                    string product1 = url1;
                    string product2 = url2;
                    return await _productComparisonPlugin.CompareProductsAsync(product1, product2);
                
            }
            return null;
            
            }
        
        }
    }