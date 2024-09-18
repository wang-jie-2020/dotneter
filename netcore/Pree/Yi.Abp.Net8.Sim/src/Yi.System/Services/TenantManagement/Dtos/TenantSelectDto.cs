using Volo.Abp.Application.Dtos;

namespace Yi.System.Services.TenantManagement.Dtos;

public class TenantSelectDto : EntityDto<Guid>
{
    public string Name { get; set; }
}