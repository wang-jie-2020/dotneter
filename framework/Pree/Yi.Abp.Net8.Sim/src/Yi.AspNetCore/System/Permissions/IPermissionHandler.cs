namespace Yi.AspNetCore.System.Permissions;

public interface IPermissionHandler
{
    bool IsPass(string permission);
}