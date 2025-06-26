using SqlSugar;

namespace Yi.Framework.SqlSugarCore;

public interface ISqlSugarDbContext
{
    ISqlSugarClient SqlSugarClient { get; }
}