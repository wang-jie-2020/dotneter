using SqlSugar;

namespace Yi.Framework.SqlSugarCore.Repositories;

public interface ISqlSugarRepository<TEntity> : ISimpleClient<TEntity>, ISugarRepository
    where TEntity : class, new()
{

}