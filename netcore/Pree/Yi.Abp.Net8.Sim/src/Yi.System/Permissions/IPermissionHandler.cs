namespace Yi.System.Permissions;

public interface IPermissionHandler
{
    bool IsPass(string permission);
}