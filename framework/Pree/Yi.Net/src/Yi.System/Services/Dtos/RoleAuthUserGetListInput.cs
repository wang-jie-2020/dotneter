using Yi.AspNetCore.Core;
using Yi.Framework.Core;

namespace Yi.System.Services.Dtos;

public class RoleAuthUserGetListInput : PagedInput
{
    public string? UserName { get; set; }

    public long? Phone { get; set; }
}