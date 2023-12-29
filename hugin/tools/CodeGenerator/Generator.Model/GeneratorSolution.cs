using System.Collections.Generic;

namespace Generator
{
    public class GeneratorSolution
    {
        public List<GeneratorProject> Projects { get; set; }

        public GeneratorProject CurrentProject { get; set; }

        public string CurrentSelectedItem { get; set; }

        public GeneratorSolution()
        {
            Projects = new List<GeneratorProject>();
        }
    }
}
