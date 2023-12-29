using Generator.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Generator
{
    public class GeneratorEntityProperty
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public List<GeneratorAttribute> Attributes { get; private set; } = new List<GeneratorAttribute>();

        public List<string> Annotations { get; private set; } = new List<string>();

        private readonly PropertyDeclarationSyntax _property;

        public GeneratorEntityProperty(PropertyDeclarationSyntax property)
        {
            _property = property;
            AnalysisSyntaxTree();
        }

        /// <summary>
        /// 简化语法树
        /// </summary>
        private void AnalysisSyntaxTree()
        {
            Name = _property.Identifier.ToString();
            Type = _property.Type.ToString();

            var attributes = _property.GetAttributes();
            Attributes = attributes.Select(attribute =>
            {
                var name = attribute.Name.ToString();
                var paras = attribute.ArgumentList?.Arguments.Select(m => m.ToString()).ToList();
                var full = attribute.ToString();

                return new GeneratorAttribute(name, paras, full);
            }).ToList();

            Annotations = _property.GetAnnotationsOfString();
        }

        public List<GeneratorAttribute> GetValidateAttributes()
        {
            var assembly = typeof(ValidationAttribute).Assembly;
            var validateAttributes = assembly.GetTypes()
                .Where(p => typeof(ValidationAttribute).IsAssignableFrom(p))
                .SelectMany(p => new List<string>
                {
                    p.Name,
                    p.FullName
                }).ToList();

            var result = new List<GeneratorAttribute>();

            foreach (var attribute in Attributes)
            {
                var attributeName = attribute.Name;

                if (!attributeName.EndsWith("Attribute"))
                {
                    attributeName += "Attribute";
                }
                if (validateAttributes.Contains(attributeName))
                {
                    result.Add(attribute);
                }
            }

            return result;
        }

        public string GetDisplayName()
        {
            var displayAttributes = new[]
            {
                "DisplayNameAttribute",
                "System.ComponentModel.DisplayNameAttribute"
            };

            var descriptionAttributes = new[]
            {
                "DescriptionAttribute",
                "System.ComponentModel.DescriptionAttribute"
            };

            foreach (var item in Attributes)
            {
                var attributeName = item.Name;
                if (!attributeName.EndsWith("Attribute"))
                {
                    attributeName += "Attribute";
                }
                if (displayAttributes.Contains(attributeName))
                {
                    if (item.Parameters.Any())
                    {
                        return item.Parameters[0].Replace("\"", "");
                    }
                }

                if (descriptionAttributes.Contains(attributeName))
                {
                    if (item.Parameters.Any())
                    {
                        return item.Parameters[0].Replace("\"", "");
                    }
                }
            }

            return string.Empty;
        }
    }
}