using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Rbac.Dtos;

public class PostGetListOutputDto : EntityDto<Guid>
{
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public bool State { get; set; }
    public string PostCode { get; set; } = string.Empty;
    public string PostName { get; set; } = string.Empty;
    public string? Remark { get; set; }

    public int OrderNum { get; set; }
}