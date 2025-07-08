namespace Yi.System.Services.Dtos;

public class MenuGetListQuery : PagedQuery
{
    public bool? State { get; set; }
    
    public string? MenuName { get; set; }
}