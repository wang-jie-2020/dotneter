using Generator.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Generator
{
    public class CodeGenerator
    {
        private CodeGenerator()
        {

        }

        private static CodeGenerator _instance;

        public static CodeGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(CodeGenerator))
                    {
                        if (_instance == null)
                        {
                            lock (typeof(CodeGenerator))
                            {
                                _instance = new CodeGenerator();
                            }
                        }

                    }
                }
                return _instance;
            }
        }

        public void Start(TempTable table)
        {
            var customCode = GenerateCustomCode(table);
            if (customCode.HasStop)
            {
                customCode.Properties.RemoveAll(p => p.Name.Equals("IsStop"));
            }

            var template = Path.Combine(Directory.GetParent(typeof(CodeGenerator).Assembly.Location).FullName, "EntityTemplate", "Entity.txt");
            var buildCode = GenerateBuildCode(template, customCode);

            customCode.Location.CreateFile(buildCode);
        }

        private CustomCode GenerateCustomCode(TempTable table)
        {
            var tableNameArray = table.Name.Split(new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries);
            var entityName = tableNameArray[tableNameArray.Length - 1];
            var otherNames = tableNameArray.Except(new[] { entityName }).ToArray();

            //大小写问题
            entityName = entityName.Substring(0, 1).ToUpper() + entityName.Substring(1, entityName.Length - 1);
            otherNames = otherNames.Select(p => p = p.Substring(0, 1).ToUpper() + p.Substring(1, p.Length - 1)).ToArray();

            /*
             *  Excel名称不可能总是满足Entity约定，比如大类-小类-明细定义
             *  Entity名称不存在二义性是基础，故考虑尽可能全，宁可长一些，同时也考虑是必要的
             */
            for (var index = otherNames.Count() - 1; index >= 0; index--)
            {
                var name = otherNames[index];
                if (entityName.StartsWith(name))
                {
                    continue;
                }

                entityName = name + entityName;
            }

            //Calculate EntityNameSpace
            var targetPositions = new List<string>();

            var assemblyNameSpace = Global.GeneratorSolution.CurrentProject.NameSpace;
            var relativePositions = Global.GeneratorSolution.CurrentSelectedItem.TrimStart(Global.GeneratorSolution.CurrentProject.Directory)
                                            .Split(new[] { '\\' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var position in relativePositions)
            {
                if (position == otherNames[0])
                {
                    break;
                }
                targetPositions.Add(position);
            }
            targetPositions.AddRange(otherNames);

            var entityNameSpace = assemblyNameSpace + "." + string.Join(".", targetPositions);
            var customCode = new CustomCode(entityNameSpace, entityName)
            {
                EntityDisplayName = table.DisplayName,
                Properties = GenerateCustomEntityProperty(table.Columns),
                Location = Path.Combine(Global.GeneratorSolution.CurrentProject.Directory,
                    string.Join("\\", targetPositions),
                    entityName + ".cs")
            };

            return customCode;
        }

        private List<CustomEntityProperty> GenerateCustomEntityProperty(List<TempColumn> columns)
        {
            var properties = new List<CustomEntityProperty>();

            foreach (var column in columns)
            {
                var attributes = new List<string>();
                var annotations = new List<string>();

                var name = column.Name.Substring(0, 1).ToUpper() + column.Name.Substring(1, column.Name.Length - 1);
                var type = GetPropertyType(column.Type);

                if (type.Item1 == typeof(int))
                {
                    if (Global.IsAutoIntToBool && name.StartsWith("is", StringComparison.CurrentCultureIgnoreCase))
                    {
                        type.Item1 = typeof(bool);
                        type.Item2 = "bool";
                    }

                    if (Global.IsAutoIntToEnum && (name.EndsWith("type", StringComparison.CurrentCultureIgnoreCase) || name.EndsWith("state", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        type.Item1 = typeof(Enum);
                        type.Item2 = name;
                    }
                }

                //AllowNull
                if (column.AllowNull && type.Item1.IsValueType)
                {
                    type.Item2 += "?";
                }
                else if (!column.AllowNull && type.Item1.IsClass)
                {
                    attributes.Add("Required");
                }

                //ValidateAttributes
                if (type.Item1 == typeof(string))
                {
                    if (column.Type.Contains("(") && column.Type.Contains(")"))
                    {
                        var tmp = column.Type.Substring(column.Type.IndexOf("(", StringComparison.Ordinal) + 1, column.Type.IndexOf(")", StringComparison.Ordinal) - column.Type.IndexOf("(", StringComparison.Ordinal) - 1);
                        if (int.TryParse(tmp, out int result))
                        {
                            attributes.Add($"MaxLength({result})");
                        }
                    }
                }
                else if (type.Item1 == typeof(decimal))
                {
                    if (column.Type.Contains("(") && column.Type.Contains(")"))
                    {
                        var tmp = column.Type.Substring(column.Type.IndexOf("(", StringComparison.Ordinal) + 1, column.Type.IndexOf(")", StringComparison.Ordinal) - column.Type.IndexOf("(", StringComparison.Ordinal) - 1);
                        if (int.TryParse(tmp.Substring(0, tmp.IndexOf(",")), out int precision))
                        {
                            attributes.Add(
                                int.TryParse(tmp.Substring(tmp.IndexOf(",", StringComparison.Ordinal) + 1),
                                    out int scale)
                                    ? $"DecimalPrecision({precision}, {scale})"
                                    : "DecimalPrecision");
                        }
                        else
                        {
                            attributes.Add("DecimalPrecision");
                        }
                    }
                    else
                    {
                        attributes.Add("DecimalPrecision");
                    }
                }

                //DescriptionAttributes
                attributes.Add($"Description(\"{column.DisplayName}\")");

                //Annotations
                annotations.Add("/// <summary>");
                annotations.Add($"/// {column.DisplayName}");
                if (Global.IsRemarkAsAnnotation && !string.IsNullOrWhiteSpace(column.Remark))
                {
                    annotations.Add($"/// {column.Remark}");
                }
                annotations.Add("/// </summary>");

                properties.Add(new CustomEntityProperty(name, type.Item2)
                {
                    DisplayName = column.DisplayName,
                    Attributes = attributes,
                    Annotations = annotations
                });
            }

            return properties;

            (Type, string) GetPropertyType(string type)
            {
                if (string.IsNullOrWhiteSpace(type))
                {
                    return (typeof(string), "string");
                }

                if (type.IndexOf("bit", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(bool), "bool");
                }

                if (type.IndexOf("bigint", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(long), "long");
                }

                if (type.IndexOf("int", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(int), "int");
                }

                if (type.IndexOf("decimal", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(decimal), "decimal");
                }

                if (type.IndexOf("datetime", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(DateTime), "DateTime");
                }

                if (type.IndexOf("varchar", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || type.IndexOf("nvarchar", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || type.IndexOf("nchar", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || type.IndexOf("char", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || type.IndexOf("ntext", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || type.IndexOf("text", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(string), "string");
                }

                if (type.IndexOf("uniqueidentifier", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || type.IndexOf("guid", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(Guid), "Guid");
                }

                if (type.IndexOf("image", StringComparison.CurrentCultureIgnoreCase) >= 0
                    || type.IndexOf("binary", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    return (typeof(byte[]), "byte[]");
                }

                return (typeof(string), "string");
            }
        }

        private string GenerateBuildCode(string template, CustomCode customCode)
        {
            var templateContent = File.ReadAllText(template, System.Text.Encoding.UTF8);
            var result = RazorEngine.RunCompile(templateContent, "excelGenerator", typeof(CustomCode), customCode);
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
