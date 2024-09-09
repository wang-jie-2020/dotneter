using Volo.Abp.Application.Dtos;

namespace Yi.Infra.TenantManagement.Dtos;

public class TenantSelectOutputDto : EntityDto<Guid>
{
    public string Name { get; set; }
}