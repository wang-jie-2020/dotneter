using SqlSugar;
using Volo.Abp.Application.Dtos;

namespace Yi.Infra.TenantManagement.Dtos;

public class TenantGetListOutputDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public int EntityVersion { get; set; }

    public string TenantConnectionString { get; set; }

    public DbType DbType { get; set; }
    public DateTime CreationTime { get; set; }
}