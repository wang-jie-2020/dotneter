using System.Reflection;
using Hugin.Platform.Localization;
using Volo.Abp.Localization;
using PermissionDefinitionProvider = Hugin.Application.Permissions.PermissionDefinitionProvider;

namespace Hugin.Platform.Permissions
{
    public class PlatformPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        protected override LocalizableString L(string name)
        {
            return LocalizableString.Create<PlatformResource>("Permission:" + name);
        }

        protected override string Root => PlatformConsts.Name;

        protected override Assembly[] Assemblies => new[] { typeof(PlatformPermissionDefinitionProvider).Assembly };
    }
}