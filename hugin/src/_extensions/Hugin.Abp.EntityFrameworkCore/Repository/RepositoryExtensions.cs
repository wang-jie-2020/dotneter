using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Z.EntityFramework.Plus;

namespace Hugin.Repository
{
    /// <summary>
    /// 仓储扩展方法类
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<int> BatchDeleteAsync<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IEntity<TKey>
        {
            var query = (IQueryable<TEntity>)repository;
            query = query.Where(predicate);

            return await query.DeleteAsync();
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="action"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<int> BatchUpdateAsync<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, TEntity>> action,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IEntity<TKey>
        {
            var query = (IQueryable<TEntity>)repository;
            query = query.Where(predicate);

            return await query.UpdateAsync(action);
        }
    }
}