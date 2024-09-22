using Volo.Abp.Application.Dtos;

namespace Yi.System.Services.System.Dtos;

public class MenuGetListInput : PagedAndSortedResultRequestDto
{
    public bool? State { get; set; }
    
    public string? MenuName { get; set; }
}