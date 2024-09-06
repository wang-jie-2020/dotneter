using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.Dtos.Task
{
    public class TaskGetListInput : PagedAllResultRequestDto
    {
        public string? JobId { get; set; }
        public string? GroupName { get; set; }
    }
}
