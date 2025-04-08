using System.Collections.Generic;

namespace Generator
{
    public class GeneratorAttribute
    {
        public string Name { get; }

        public List<string> Parameters { get; }

        public string FullString { get; }

        public GeneratorAttribute(string name, List<string> parameters, string fullString)
        {
            Name = name;
            Parameters = parameters;
            FullString = fullString;
        }
    }
}
