using Yi.AspNetCore.System;

namespace Yi.Sys.Services.Monitor.Dtos;

public class JobGetListInput : PagedInput
{
    public string? JobId { get; set; }
    
    public string? GroupName { get; set; }
}