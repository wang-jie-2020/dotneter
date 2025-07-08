using Yi.Framework.Loggings;

namespace Yi.System.Services.Dtos;

public class OperLogQuery : PagedQuery
{
    public OperLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}