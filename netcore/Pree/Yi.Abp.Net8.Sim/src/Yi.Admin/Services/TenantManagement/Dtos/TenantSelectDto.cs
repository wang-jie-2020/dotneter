using Volo.Abp.Application.Dtos;

namespace Yi.Admin.Services.TenantManagement.Dtos;

public class TenantSelectDto : EntityDto<Guid>
{
    public string Name { get; set; }
}