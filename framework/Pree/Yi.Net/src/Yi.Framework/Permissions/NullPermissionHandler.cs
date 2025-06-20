using Microsoft.AspNetCore.Http;
using Yi.AspNetCore.Security;
using Yi.Framework.Utils;

namespace Yi.Framework.Permissions;

public class NullPermissionHandler : IPermissionHandler
{
    public bool IsPass(string permission)
    {
        return false;
    }
}