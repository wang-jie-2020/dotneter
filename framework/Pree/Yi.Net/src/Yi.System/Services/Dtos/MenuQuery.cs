namespace Yi.System.Services.Dtos;

public class MenuQuery : PagedQuery
{
    public bool? State { get; set; }
    
    public string? MenuName { get; set; }
}