using Volo.Abp.Application.Dtos;

namespace Yi.System.Services.System.Dtos;

public class PostGetListInput : PagedAndSortedResultRequestDto
{
    public bool? State { get; set; }

    public string? PostCode { get; set; } = string.Empty;

    public string? PostName { get; set; } = string.Empty;
}