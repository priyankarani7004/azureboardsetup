using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semantickernal
{
    public class CustomProductPlugin
    {
        public static string CompareProducts(string input)
        {
            return $"Comparing: {input}\n- iPhone 15: A17 chip, iOS\n- Galaxy S23: Snapdragon 8 Gen 2, Android\n";
        }

        public static string SummarizeComparison(string input)
        {
            return $"Summary: If you want smooth integration, go iPhone. Prefer customization? Galaxy S23.";
        }
    }
}
