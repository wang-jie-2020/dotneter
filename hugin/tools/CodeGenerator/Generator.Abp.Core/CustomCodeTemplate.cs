using System;
using System.Collections.Generic;
using System.IO;

namespace Generator
{
    public class CustomCodeTemplate
    {
        public string FilePath { get; private set; }

        public string AssemblyName { get; set; }

        public CustomCodeTemplateType Type { get; private set; }

        private CustomCodeTemplate()
        {

        }

        public static List<CustomCodeTemplate> CreateCodeTemplate(string templateDir)
        {
            var result = new List<CustomCodeTemplate>();

            var files = System.IO.Directory.GetFiles(templateDir, "*.txt", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                if (Enum.TryParse(fileName, true, out CustomCodeTemplateType type))
                {
                    result.Add(new CustomCodeTemplate()
                    {
                        Type = type,
                        FilePath = file,
                        AssemblyName = System.IO.Directory.GetParent(file)?.Name
                    });
                }
            }

            return result;
        }
    }
}
