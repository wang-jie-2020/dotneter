using Volo.Abp.Application.Dtos;
using Yi.AspNetCore.Core;

namespace Yi.System.Services.Dtos;

public class RoleAuthUserGetListInput : PagedInput
{
    public string? UserName { get; set; }

    public long? Phone { get; set; }
}