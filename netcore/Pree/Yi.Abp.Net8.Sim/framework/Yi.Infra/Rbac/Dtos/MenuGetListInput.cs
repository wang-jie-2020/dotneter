using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Rbac.Dtos;

public class MenuGetListInput : PagedAndSortedResultRequestDto
{
    public bool? State { get; set; }
    public string? MenuName { get; set; }
}