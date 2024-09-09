using Volo.Abp.Uow;
using Yi.Framework.SqlSugarCore.Abstractions;

namespace Yi.Framework.SqlSugarCore.Uow;

public class SqlSugarDatabaseApi : IDatabaseApi
{
    public SqlSugarDatabaseApi(ISqlSugarDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public ISqlSugarDbContext DbContext { get; }
}