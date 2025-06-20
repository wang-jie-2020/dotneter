using Yi.AspNetCore.Core;
using Yi.Framework.Core;
using Yi.Framework.Loggings;

namespace Yi.System.Monitor.Dtos;

public class OperLogGetListInput : PagedInput
{
    public OperLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}