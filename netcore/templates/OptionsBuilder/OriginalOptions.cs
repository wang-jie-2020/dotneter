using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Demo
{
    public class OriginalOptions
    {
        public OriginalOptions()
        {
            Extensions = new List<string>();
        }

        [AllowNull]
        public string Name { get; set; }

        [AllowNull]
        public string Value { get; set; }

        public List<string> Extensions { get; set; }
    }
}
