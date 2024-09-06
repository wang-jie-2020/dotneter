using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Rbac.Dtos.Post
{
    public class PostGetListInputVo : PagedAndSortedResultRequestDto
    {
        public bool? State { get; set; }
        //public string? PostCode { get; set; }=string.Empty;
        public string? PostName { get; set; } = string.Empty;
    }
}
