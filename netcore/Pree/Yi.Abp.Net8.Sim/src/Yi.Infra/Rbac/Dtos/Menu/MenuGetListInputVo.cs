using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Rbac.Dtos.Menu
{
    public class MenuGetListInputVo : PagedAndSortedResultRequestDto
    {

        public bool? State { get; set; }
        public string? MenuName { get; set; }

    }
}
