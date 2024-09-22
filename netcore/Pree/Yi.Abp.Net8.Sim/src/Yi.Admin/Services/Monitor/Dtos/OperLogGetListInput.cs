using Yi.AspNetCore.System;
using Yi.AspNetCore.System.Loggings;

namespace Yi.Admin.Services.Monitor.Dtos;

public class OperLogGetListInput : PagedInfraInput
{
    public OperLogEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}