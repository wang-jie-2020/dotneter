using System.Collections.Generic;

namespace Generator
{
    public class CustomEntityProperty
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string DisplayName { get; set; }

        public List<string> Annotations { get; set; } = new List<string>();

        public List<string> Attributes { get; set; } = new List<string>();

        public CustomEntityProperty(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
