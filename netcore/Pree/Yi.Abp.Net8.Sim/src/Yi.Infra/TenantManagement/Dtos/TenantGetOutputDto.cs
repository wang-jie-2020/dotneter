using Volo.Abp.Application.Dtos;

namespace Yi.Abp.Infra.TenantManagement.Dtos
{
    public class TenantGetOutputDto:EntityDto<Guid>
    {
        public  string Name { get;  set; }
        public int EntityVersion { get;  set; }

        public string TenantConnectionString { get;  set; }

        public SqlSugar.DbType DbType { get;  set; }

        public DateTime CreationTime { get; set; }
    }
}
