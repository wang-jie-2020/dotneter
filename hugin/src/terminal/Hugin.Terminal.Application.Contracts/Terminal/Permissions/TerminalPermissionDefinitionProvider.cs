using System.Reflection;
using Hugin.Terminal.Localization;
using Volo.Abp.Localization;
using PermissionDefinitionProvider = Hugin.Application.Permissions.PermissionDefinitionProvider;

namespace Hugin.Terminal.Permissions
{
    public class TerminalPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        protected override LocalizableString L(string name)
        {
            return LocalizableString.Create<TerminalResource>("Permission:" + name);
        }

        protected override string Root => TerminalConsts.Name;

        protected override Assembly[] Assemblies => new[] { typeof(TerminalPermissionDefinitionProvider).Assembly };
    }
}