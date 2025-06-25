using System.Linq.Expressions;
using SqlSugar;

namespace Yi.Framework.SqlSugarCore.Repositories;

public interface ISqlSugarRepository<TEntity> : ISimpleClient<TEntity>, ISugarRepository
    where TEntity : class, new()
{

}

public interface ISqlSugarRepository<TEntity, TKey> : ISqlSugarRepository<TEntity>
    where TEntity : class, IEntity<TKey>, new()
{
    Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);

    Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(TKey id, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<TEntity> GetAsync(TKey id, bool includeDetails = true,
        CancellationToken cancellationToken = default);
}