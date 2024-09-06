namespace Yi.Infra.Rbac.Authorization
{
    public interface IPermissionHandler
    {
        bool IsPass(string permission);
    }
}
