// using System.Linq.Expressions;
// using SqlSugar;
// using Volo.Abp.Domain.Entities;
// using Volo.Abp.Domain.Repositories;
// using Volo.Abp.Linq;
//
// namespace Yi.AspNetCore.SqlSugarCore.Repositories;
//
// public class SqlSugarObjectRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
// {
//     public IAsyncQueryableExecuter AsyncExecuter => throw new NotImplementedException();
//
//     public bool? IsChangeTrackingEnabled => throw new NotImplementedException();
//
//     public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,
//         bool includeDetails = false, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<IQueryable<TEntity>> GetQueryableAsync()
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public IQueryable<TEntity> WithDetails()
//     {
//         throw new NotImplementedException();
//     }
//
//     public IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<IQueryable<TEntity>> WithDetailsAsync()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
//     {
//         throw new NotImplementedException();
//     }
//     
//     public virtual async Task<ISqlSugarClient> GetDbContextAsync()
//     {
//         throw new NotImplementedException();
//     }
// }
//
// public class SqlSugarObjectRepository<TEntity, TKey> : SqlSugarObjectRepository<TEntity>, IRepository<TEntity, TKey>
//     where TEntity : class, IEntity<TKey>
// {
//     public Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false,
//         CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
// }