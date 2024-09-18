using Yi.AspNetCore.Common;

namespace Yi.Admin.Services.Dtos;

public class JobGetListInput : PagedInfraInput
{
    public string? JobId { get; set; }
    
    public string? GroupName { get; set; }
}