using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Rbac.Dtos.Role
{
    public class RoleAuthUserGetListInput : PagedAndSortedResultRequestDto
    {
        public string? UserName { get; set; }

        public long? Phone { get; set; }
    }
}
