namespace Yi.AspNetCore.Authorization;

public interface IPermissionCheckHandler
{
    Task<bool> CheckAsync(PermissionCheckContext context);
}