using Yi.AspNetCore.System;

namespace Yi.Sys.Services.Infra.Dtos;

public class DeptGetListInput : PagedInput
{
    public Guid Id { get; set; }
    
    public bool? State { get; set; }
    
    public string? DeptName { get; set; }
    
    public string? DeptCode { get; set; }
    
    public string? Leader { get; set; }
}