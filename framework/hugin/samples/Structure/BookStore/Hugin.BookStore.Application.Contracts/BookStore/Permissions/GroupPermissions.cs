using Hugin.Application.Permissions;
using Volo.Abp.MultiTenancy;

namespace Hugin.BookStore.Permissions
{
    /// <summary>
    /// <see cref="BookStorePermissionDefinitionProvider"/>
    /// </summary>
    public partial class BookStorePermissions : IPermissionDefine
    {
        public static class Group
        {
            public const string Default = BookStoreConsts.Name + ".BookStore";
        }

        public MultiTenancySides MultiTenancySide { get; } = MultiTenancySides.Tenant;
    }
}