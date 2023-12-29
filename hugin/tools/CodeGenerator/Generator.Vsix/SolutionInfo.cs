using Generator;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using System.IO;
using System.Linq;

namespace VsAssistant
{
    public class SolutionInfo
    {
        public Solution Solution { get; private set; }

        public Project[] Projects { get; private set; }

        public SolutionInfo()
        {
            var workspace = ((IComponentModel)Package.GetGlobalService(typeof(SComponentModel))).GetService<VisualStudioWorkspace>();
            Solution = workspace.CurrentSolution;
            Projects = workspace.CurrentSolution.Projects.ToArray();
        }

        public GeneratorSolution CreateGeneratorSolution(string project, string item)
        {
            var generatorSolution = new GeneratorSolution();

            foreach (var p in Projects)
            {
                var generatorProject = new GeneratorProject()
                {
                    Name = p.Name,
                    NameSpace = p.DefaultNamespace,
                    Directory = Directory.GetParent(p.FilePath).FullName
                };
                generatorSolution.Projects.Add(generatorProject);

                if (p.FilePath == project)
                {
                    generatorSolution.CurrentProject = generatorProject;
                    generatorSolution.CurrentSelectedItem = item;
                }
            }

            return generatorSolution;
        }
    }
}