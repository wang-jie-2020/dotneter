using Yi.System.Domains.Entities;

namespace Yi.System.Domains.Repositories;

public class SqlSugarTenantRepository : SqlSugarRepository<TenantEntity>, ISqlSugarTenantRepository, ITransientDependency
{
    public SqlSugarTenantRepository(ISugarDbContextProvider<ISqlSugarDbContext> sugarDbContextProvider) : base(
        sugarDbContextProvider)
    {
    }

    public async Task<TenantEntity> FindByNameAsync(string name, bool includeDetails = true)
    {
        return await AsQueryable().FirstAsync(x => x.Name == name);
    }

    public async Task<long> GetCountAsync(string? filter = null)
    {
        return await AsQueryable().WhereIF(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter)).CountAsync();
    }

    public async Task<List<TenantEntity>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false)
    {
        return await AsQueryable().WhereIF(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .OrderByIF(!string.IsNullOrEmpty(sorting), sorting)
            .ToPageListAsync(skipCount, maxResultCount);
    }
}