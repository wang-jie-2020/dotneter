using Hugin.Application.Permissions;
using Volo.Abp.MultiTenancy;

namespace Hugin.Platform.Permissions
{
    public partial class PlatformPermissions : IPermissionDefine
    {
        public static class Group
        {
            public const string Default = PlatformConsts.Name;
        }

        public MultiTenancySides MultiTenancySide { get; } = MultiTenancySides.Tenant;
    }
}
