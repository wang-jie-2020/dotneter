namespace Yi.Framework.Permissions;

public class NullPermissionHandler : IPermissionHandler
{
    public bool IsPass(string permission)
    {
        return false;
    }
}