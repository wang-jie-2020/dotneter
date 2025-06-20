namespace Yi.Framework.SqlSugarCore;

public interface ISugarDbContextProvider<TDbContext>
    where TDbContext : ISqlSugarDbContext
{
    Task<TDbContext> GetDbContextAsync();
}