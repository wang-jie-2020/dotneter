using Volo.Abp.Application.Dtos;

namespace Yi.Infra.TenantManagement.Dtos;

public class TenantSelectDto : EntityDto<Guid>
{
    public string Name { get; set; }
}