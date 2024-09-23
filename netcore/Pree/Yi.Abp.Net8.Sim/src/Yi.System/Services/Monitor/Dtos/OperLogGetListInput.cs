using Yi.AspNetCore.System;
using Yi.AspNetCore.System.Loggings;

namespace Yi.System.Services.Monitor.Dtos;

public class OperLogGetListInput : PagedInput
{
    public OperLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}