using Volo.Abp.Application.Dtos;

namespace Yi.System.Services.Rbac.Dtos;

public class RoleAuthUserGetListInput : PagedAndSortedResultRequestDto
{
    public string? UserName { get; set; }

    public long? Phone { get; set; }
}