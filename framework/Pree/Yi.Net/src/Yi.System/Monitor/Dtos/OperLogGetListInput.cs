using Yi.AspNetCore.Core;
using Yi.AspNetCore.Core.Loggings;
using Yi.Framework.Core;

namespace Yi.System.Monitor.Dtos;

public class OperLogGetListInput : PagedInput
{
    public OperLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}