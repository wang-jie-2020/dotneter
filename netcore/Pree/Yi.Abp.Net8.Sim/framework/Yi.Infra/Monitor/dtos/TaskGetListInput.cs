namespace Yi.Infra.Monitor.dtos;

public class TaskGetListInput : PagedInfraInput
{
    public string? JobId { get; set; }
    
    public string? GroupName { get; set; }
}