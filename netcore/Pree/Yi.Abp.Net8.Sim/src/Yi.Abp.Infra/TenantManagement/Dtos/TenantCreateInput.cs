
namespace Yi.Abp.Infra.TenantManagement.Dtos
{
    public class TenantCreateInput
    {
        public  string Name { get;  set; }
        public string TenantConnectionString { get;  set; }

        public SqlSugar.DbType DbType { get;  set; }
    }
}
