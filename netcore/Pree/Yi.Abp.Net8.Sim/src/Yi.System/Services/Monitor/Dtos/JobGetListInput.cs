namespace Yi.System.Services.Monitor.Dtos;

public class JobGetListInput : PagedInfraInput
{
    public string? JobId { get; set; }
    
    public string? GroupName { get; set; }
}