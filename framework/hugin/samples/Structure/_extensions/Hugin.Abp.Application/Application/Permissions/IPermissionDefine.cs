using Volo.Abp.MultiTenancy;

namespace Hugin.Application.Permissions
{
    public interface IPermissionDefine
    {
        MultiTenancySides MultiTenancySide { get; }
    }
}
