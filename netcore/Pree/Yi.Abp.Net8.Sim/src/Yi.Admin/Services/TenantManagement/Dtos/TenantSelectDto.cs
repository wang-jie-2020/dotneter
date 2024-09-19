using Volo.Abp.Application.Dtos;

namespace Yi.Admin.Services.TenantManagement.Dtos;

public class TenantSelectDto 
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
}