using Volo.Abp.Collections;

namespace Yi.AspNetCore.Authorization;

public class PermissionOptions
{
    public TypeList<IPermissionCheckHandler> CheckHandlers { get; }
    
    public PermissionOptions()
    {
        CheckHandlers = new TypeList<IPermissionCheckHandler>();
    }
}