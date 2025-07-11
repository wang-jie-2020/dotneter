using Yi.AspNetCore.Mvc.OperLogging;

namespace Yi.System.Services.Dtos;

public class OperLogQuery : PagedQuery
{
    public OperLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}