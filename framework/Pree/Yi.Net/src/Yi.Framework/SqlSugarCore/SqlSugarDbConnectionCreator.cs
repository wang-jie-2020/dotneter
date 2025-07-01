using Microsoft.Extensions.Options;
using SqlSugar;
using Yi.AspNetCore.Data;

namespace Yi.Framework.SqlSugarCore;

public class SqlSugarDbConnectionCreator : ISqlSugarDbConnectionCreator
{
    private readonly SqlSugarConnectionOptions _sqlSugarConnectionOptions;
    
    public SqlSugarDbConnectionCreator(IOptions<SqlSugarConnectionOptions> options)
    {
        _sqlSugarConnectionOptions = options.Value;
    }
    
    public List<ConnectionConfig> Build(Action<ConnectionConfig>? action = null)
    {
        var connectionConfig = new ConnectionConfig
        {
            ConfigId = ConnectionStrings.DefaultConnectionStringName,
            IsAutoCloseConnection = true,
        };
        
        action?.Invoke(connectionConfig);

        var list = new List<ConnectionConfig> { connectionConfig };

        if ( _sqlSugarConnectionOptions.SqlSugarConnectionConfigs?.Count > 0)
        {
            list.AddRange(_sqlSugarConnectionOptions.SqlSugarConnectionConfigs);
        }

        return list;
    }
}