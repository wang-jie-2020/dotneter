using Yi.Framework.Core;

namespace Yi.System.Services.Dtos;

public class DeptGetListInput : PagedInput
{
    public Guid Id { get; set; }
    
    public bool? State { get; set; }
    
    public string? DeptName { get; set; }
    
    public string? DeptCode { get; set; }
    
    public string? Leader { get; set; }
}