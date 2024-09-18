using Volo.Abp.Application.Dtos;

namespace Yi.System.Services.Rbac.Dtos;

public class MenuGetListInput : PagedAndSortedResultRequestDto
{
    public bool? State { get; set; }
    
    public string? MenuName { get; set; }
}