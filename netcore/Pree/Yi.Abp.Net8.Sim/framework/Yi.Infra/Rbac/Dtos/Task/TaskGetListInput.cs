namespace Yi.Infra.Rbac.Dtos.Task;

public class TaskGetListInput : PagedInfraInput
{
    public string? JobId { get; set; }
    public string? GroupName { get; set; }
}