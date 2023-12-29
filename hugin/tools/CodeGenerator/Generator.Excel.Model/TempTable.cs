using System.Collections.Generic;

namespace Generator
{
    public class TempTable
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<TempColumn> Columns { get; set; } = new List<TempColumn>();
    }
}
