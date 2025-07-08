namespace Yi.System.Services.Dtos;

public class DeptQuery : PagedQuery
{
    public bool? State { get; set; }
    
    public string? DeptName { get; set; }
}