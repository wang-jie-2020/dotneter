namespace Yi.Infra.Monitor.Dtos;

public class TaskGetListInput : PagedInfraInput
{
    public string? JobId { get; set; }
    
    public string? GroupName { get; set; }
}