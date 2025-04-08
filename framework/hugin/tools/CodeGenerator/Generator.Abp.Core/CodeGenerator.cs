using Generator.Extensions;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Generator
{
    public class CodeGenerator
    {
        public void Start()
        {
            var templates = CustomFeature.GetTemplates();

            var excepts = new List<CustomCodeTemplateType>();
            if (!Global.Options.UseApplication)
            {
                excepts.AddRange(new[]
                {
                    CustomCodeTemplateType.EditDto,
                    CustomCodeTemplateType.ListDto,
                    CustomCodeTemplateType.QueryInput,
                    CustomCodeTemplateType.ObjectMapper,
                    CustomCodeTemplateType.Permission,
                    CustomCodeTemplateType.GroupPermission,
                    CustomCodeTemplateType.Service,
                    CustomCodeTemplateType.ServiceInterface
                });
            }
            if (!Global.Options.UseDomain)
            {
                excepts.AddRange(new[]
                {
                    CustomCodeTemplateType.GroupManager,
                    CustomCodeTemplateType.GroupManagerInterface,
                    CustomCodeTemplateType.GroupErrCode
                });
            }
            if (!Global.Options.UseEntityFramework)
            {
                excepts.AddRange(new[]
                {
                    CustomCodeTemplateType.EntityFrameworkConfiguration
                });
            }
            templates.RemoveAll(p => excepts.Contains(p.Type));

            var customCode = GenerateCustomCode();
            foreach (var template in templates)
            {
                var buildCode = GenerateBuildCode(template, customCode);
                var buildLocation = CodeLocator.Locator(template, customCode, buildCode);

                //不存在则输出
                if (!File.Exists(buildLocation))
                {
                    buildLocation.CreateFile(buildCode);
                    continue;
                }

                //Group存在不输出
                if (template.Type.ToString().Contains("Group"))
                {
                    continue;
                }

                if (Global.Options.IsOverride)
                {
                    buildLocation.CreateFile(buildCode);
                    continue;
                }
            }
        }

        private CustomCode GenerateCustomCode()
        {
            var customCode = new CustomCode(Global.Entity.NameSpace,
                                            Global.GeneratorSolution.CurrentProject.NameSpace,
                                            Global.Entity.Name)
            {
                EntityDisplayName = Global.EntityDto?.DisplayName,
                HasStop = Global.Entity.Bases.Any(p => p.Contains("stop") || p.Contains("Stop"))
            };

            if (Global.Options.UseApplication)
            {
                foreach (var dtoProperty in Global.EntityDto.Properties.Where(p => p.Checked))
                {
                    var entityProperty = Global.Entity.Properties.First(o => o.Name == dtoProperty.Name);
                    var customProperty = new CustomEntityProperty(entityProperty.Name, entityProperty.Type)
                    {
                        DisplayName = dtoProperty.DisplayName,
                        Annotations = dtoProperty.Annotations,
                        ValidateAttributes = entityProperty.GetValidateAttributes().Select(p => p.FullString).ToList()
                    };

                    customCode.Properties.Add(customProperty);
                }
            }

            return customCode;
        }

        private string GenerateBuildCode(CustomCodeTemplate template, CustomCode customCode)
        {
            var templateContent = File.ReadAllText(template.FilePath, System.Text.Encoding.UTF8);
            var result = RazorEngine.RunCompile(templateContent, template.Type.ToString(), typeof(CustomCode), customCode);
            return result
                .Replace("&#39;", "'")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&lt;", "<")
                .Replace("<pre>", "")
                .Replace("</pre>", "");
        }

        private IRazorEngineService _razorEngine;
        public IRazorEngineService RazorEngine
        {
            get
            {
                if (_razorEngine == null)
                {
                    var templateServiceConfiguration = new TemplateServiceConfiguration()
                    {
                        Language = Language.CSharp,
                        DisableTempFileLocking = true,
                        CachingProvider = new DefaultCachingProvider(delegate (string t)
                        {
                            Directory.Delete(t, true);
                        })
                    };

                    _razorEngine = Engine.Razor = RazorEngineService.Create(templateServiceConfiguration);
                }
                return _razorEngine;
            }
        }
    }
}
