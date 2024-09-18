using Volo.Abp.Application.Dtos;

namespace Yi.System.TenantManagement.Dtos;

public class TenantSelectDto : EntityDto<Guid>
{
    public string Name { get; set; }
}