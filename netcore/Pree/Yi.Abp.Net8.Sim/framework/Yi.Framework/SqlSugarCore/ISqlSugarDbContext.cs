using SqlSugar;

namespace Yi.Framework.SqlSugarCore;

public interface ISqlSugarDbContext
{
    ISqlSugarClient SqlSugarClient { get; }
    
    DbConnOptions Options { get; }
    
    void SetSqlSugarClient(ISqlSugarClient sqlSugarClient);
}