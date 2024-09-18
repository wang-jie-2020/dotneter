namespace Yi.AspNetCore.Permissions;

public interface IPermissionHandler
{
    bool IsPass(string permission);
}