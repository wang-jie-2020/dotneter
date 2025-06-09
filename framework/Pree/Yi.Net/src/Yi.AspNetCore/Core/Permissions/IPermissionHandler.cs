namespace Yi.AspNetCore.Core.Permissions;

public interface IPermissionHandler
{
    bool IsPass(string permission);
}