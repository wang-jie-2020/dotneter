using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Infra.TenantManagement.Dtos;

public class TenantGetListInput : PagedAllResultRequestDto
{
    public string? Name { get; set; }
    //public int? EntityVersion { get;  set; }

    //public string? TenantConnectionString { get;  set; }

    //public DbType? DbType { get;  set; }
}