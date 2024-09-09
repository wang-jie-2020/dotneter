using Yi.Framework.Ddd.Application;

namespace Yi.Infra.Rbac.Dtos.Task;

public class TaskGetListInput : PagedAllResultRequestDto
{
    public string? JobId { get; set; }
    public string? GroupName { get; set; }
}