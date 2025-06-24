using Volo.Abp.DependencyInjection;

namespace Yi.Framework.Permissions;

[Dependency(TryRegister = true)]
public class NullPermissionHandler : IPermissionHandler, ITransientDependency
{
    public bool IsPass(string permission)
    {
        return false;
    }
}