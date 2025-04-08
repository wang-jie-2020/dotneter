using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Extensions
{
    public static class SyntaxExtensions
    {
        public static List<T> GetCSharpSyntaxNode<T>(this CSharpSyntaxNode node) where T : CSharpSyntaxNode
        {
            return node
                .DescendantNodes(p => !(p is T))
                .OfType<T>()
                .ToList();
        }

        public static List<PropertyDeclarationSyntax> GetProperties(this ClassDeclarationSyntax classNode, bool includingAncestor = false)
        {
            var properties = classNode.GetCSharpSyntaxNode<PropertyDeclarationSyntax>()
                .Where(p => p.Modifiers.Any(m =>
                            m.Kind() == SyntaxKind.PublicKeyword) &&
                            p.AccessorList != null &&
                            p.AccessorList.Accessors.Any(a => a.Kind() == SyntaxKind.GetAccessorDeclaration));

            return properties
                .Where(p => includingAncestor || p.FirstAncestorOrSelf((Func<ClassDeclarationSyntax, bool>)null) == classNode)
                .ToList();
        }

        public static List<AttributeSyntax> GetAttributes(this MemberDeclarationSyntax node)
        {
            var result = new List<AttributeSyntax>();
            foreach (var item in node.AttributeLists)
            {
                result.AddRange(item.Attributes.ToList());
            }

            return result;
        }

        public static List<SyntaxTrivia> GetAnnotationsOfSyntaxTrivia(this SyntaxNode node)
        {
            return node.GetLeadingTrivia().ToList();
        }

        public static List<string> GetAnnotationsOfString(this SyntaxNode node)
        {
            var annotation = node.GetLeadingTrivia().ToFullString();
            var arr = annotation.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i].Trim();
            }

            return arr.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
        }
    }
}
