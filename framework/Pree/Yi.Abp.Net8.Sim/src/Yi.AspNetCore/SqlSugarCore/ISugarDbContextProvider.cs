namespace Yi.AspNetCore.SqlSugarCore;

public interface ISugarDbContextProvider<TDbContext>
    where TDbContext : ISqlSugarDbContext
{
    Task<TDbContext> GetDbContextAsync();
}