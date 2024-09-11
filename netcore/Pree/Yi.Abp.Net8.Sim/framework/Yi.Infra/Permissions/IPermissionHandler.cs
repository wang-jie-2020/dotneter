namespace Yi.Infra.Permissions;

public interface IPermissionHandler
{
    bool IsPass(string permission);
}