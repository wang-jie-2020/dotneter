using SqlSugar;

namespace Yi.Framework.SqlSugarCore;

/// <summary>
///  Sugar 补充连接
/// </summary>
public class SqlSugarConnectionOptions
{
    public List<ConnectionConfig> SqlSugarConnectionConfigs { get; set; }
}