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

}