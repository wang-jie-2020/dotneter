namespace Yi.Framework.Permissions;

public interface IPermissionHandler
{
    bool IsPass(string permission);
}