using SqlSugar;

namespace Yi.Framework.SqlSugarCore;

public class DbConnOptions
{
    /// <summary>
    ///     连接字符串(如果开启多租户，也就是默认库了)，必填
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    ///     数据库类型
    /// </summary>
    public DbType? DbType { get; set; }
}

//public sealed class SqlSugarConnectionOptions
//{
//    public List<DbConnectionConfig> ConnectionConfigs { get; set; }
//}

//public sealed class DbConnectionConfig : ConnectionConfig
//{

//}
