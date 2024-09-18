using SqlSugar;

namespace Yi.AspNetCore.SqlSugarCore;

public interface ISqlSugarDbContext
{
    ISqlSugarClient SqlSugarClient { get; }
    
    DbConnOptions Options { get; }
    
    void SetSqlSugarClient(ISqlSugarClient sqlSugarClient);
}