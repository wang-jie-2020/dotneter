using SqlSugar;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Api.Entry;

[DependsOn(typeof(AbpAutofacModule))]
public class Entry : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddScoped<ISqlSugarClient>(_ =>
        {
            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = DbType.PostgreSQL,
                    ConnectionString = configuration.GetConnectionString("Default"),
                    IsAutoCloseConnection = true,
                },
                db =>
                {

                });
            return sqlSugar;
        });
    }
}