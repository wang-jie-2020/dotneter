using Generator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public class GeneratorEntity
    {
        public List<string> Usings { get; private set; } = new List<string>();

        public string NameSpace { get; private set; }

        public string Name { get; private set; }

        public List<string> Bases { get; private set; } = new List<string>();

        public List<GeneratorAttribute> Attributes { get; private set; } = new List<GeneratorAttribute>();

        public List<GeneratorEntityProperty> Properties { get; protected set; } = new List<GeneratorEntityProperty>();

        public List<string> Annotations { get; private set; } = new List<string>();

        private readonly SyntaxTree _tree;

        public GeneratorEntity(string classContent)
        {
            _tree = CSharpSyntaxTree.ParseText(classContent);
            AnalysisSyntaxTree();
        }

        /// <summary>
        /// 简化语法树
        /// </summary>
        private void AnalysisSyntaxTree()
        {
            var root = _tree.GetRoot() as CSharpSyntaxNode;

            var usings = root.GetCSharpSyntaxNode<UsingDirectiveSyntax>();
            Usings = usings.Select(p => p.ToString()).ToList();

            var nameSpace = root.GetCSharpSyntaxNode<NamespaceDeclarationSyntax>().First();
            NameSpace = nameSpace.Name.ToString();

            var classDeclaration = root.GetCSharpSyntaxNode<ClassDeclarationSyntax>().First();
            Name = classDeclaration.Identifier.ToString();

            var bases = classDeclaration.BaseList?.Types.ToList();
            Bases = bases?.Select(p => p.ToString()).ToList();

            var attributes = classDeclaration.GetAttributes();
            Attributes = attributes.Select(attribute =>
            {
                var name = attribute.Name.ToString();
                var paras = attribute.ArgumentList?.Arguments.Select(m => m.ToString()).ToList();
                var full = attribute.ToString();

                return new GeneratorAttribute(name, paras, full);
            }).ToList();

            var properties = classDeclaration.GetProperties();
            Properties = properties.Select(property => new GeneratorEntityProperty(property)).ToList();

            Annotations = classDeclaration.GetAnnotationsOfString();
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