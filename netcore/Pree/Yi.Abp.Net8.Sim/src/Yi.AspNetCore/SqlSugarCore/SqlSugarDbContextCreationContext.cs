using System.Data.Common;

namespace Yi.AspNetCore.SqlSugarCore;

public class SqlSugarDbContextCreationContext
{
    private static readonly AsyncLocal<SqlSugarDbContextCreationContext> _current = new();

    public SqlSugarDbContextCreationContext(string connectionStringName, string connectionString)
    {
        ConnectionStringName = connectionStringName;
        ConnectionString = connectionString;
    }

    public static SqlSugarDbContextCreationContext Current => _current.Value;
    public string ConnectionStringName { get; }

    public string ConnectionString { get; }

    public DbConnection ExistingConnection { get; internal set; }

    public static IDisposable Use(SqlSugarDbContextCreationContext context)
    {
        var previousValue = Current;
        _current.Value = context;
        return new DisposeAction(() => _current.Value = previousValue);
    }
}