using Yi.AspNetCore.Core;
using Yi.AspNetCore.Core.Loggings;

namespace Yi.Sys.Services.Monitor.Dtos;

public class OperLogGetListInput : PagedInput
{
    public OperLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}