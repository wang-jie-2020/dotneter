using Generator.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Linq;

namespace Generator
{
    public class CodeLocator
    {
        /// <summary>
        /// 定位目标程序集的目标位置
        /// </summary>
        /// <param name="buildCode"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string Locator(CustomCodeTemplate template, CustomCode customCode, string buildCode)
        {
            GeneratorProject targetProject = GetTargetProject(template);
            if (targetProject == null)
            {
                throw new System.Exception("找不到输出程序集");
            }

            var buildCodeTree = CSharpSyntaxTree.ParseText(buildCode);

            var root = buildCodeTree.GetRoot() as CSharpSyntaxNode;
            var buildNameSpace = root.GetCSharpSyntaxNode<NamespaceDeclarationSyntax>().First().Name.ToString();

            var buildName = root.GetCSharpSyntaxNode<ClassDeclarationSyntax>().Any()
                ? root.GetCSharpSyntaxNode<ClassDeclarationSyntax>().First().Identifier.ToString()
                : root.GetCSharpSyntaxNode<InterfaceDeclarationSyntax>().First().Identifier.ToString();

            var targetFullName = Path.Combine(targetProject.Directory,
                Path.Combine(buildNameSpace.TrimStart(targetProject.NameSpace)
                                .Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries)),
                buildName + ".cs");

            return CustomFeature.GetLocator(template, customCode, targetFullName);
        }

        private static GeneratorProject GetTargetProject(CustomCodeTemplate template)
        {
            GeneratorProject target = null;

            var targerName = Global.GeneratorSolution.CurrentProject.Name;
            while (!string.IsNullOrEmpty(targerName))
            {
                target = Global.GeneratorSolution.Projects.FirstOrDefault(p => p.Name == targerName + "." + template.AssemblyName);
                if (target != null)
                {
                    break;
                }

                targerName = GetBeforeLastDot(targerName);
            }

            return target;
        }

        private static string GetBeforeLastDot(string input)
        {
            var index = input.LastIndexOf(".");
            if (index == -1)
            {
                return string.Empty;
            }

            return input.Substring(0, index);
        }
    }
}
