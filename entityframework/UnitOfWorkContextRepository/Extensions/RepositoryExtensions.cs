using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;

namespace UnitOfWorkContextRepository.Extensions
{
    /// <summary>
    ///     这里的批量操作实际与EntityFramework没什么关系,真正的实现不是基于EFCore的
    ///             -SQLServer (or SqlAzure) under the hood uses SqlBulkCopy for Insert, Update/Delete = BulkInsert + raw Sql MERGE.
    ///             -PostgreSQL (9.5+) is using COPY BINARY combined with ON CONFLICT for Update (supported from v6+).
    ///             -MySQL (8+) is using MySqlBulkCopy combined with ON DUPLICATE for Update (Only Bulk ops supported from v6+).
    ///             -SQLite has no Copy tool, instead library uses plain SQL combined with UPSERT.
    ///     需要指出的是这样的变更是不会由EFCore进行追踪的,故这里需要注意的是定义在EFCore的查询筛选或者默认赋值
    ///     通常的场景是通过EFCore查询或者直接新建对象,通过批量操作保存至数据库
    /// </summary>
    public static class RepositoryExtensions
    {
        public static async Task<int> BatchUpdateAsync<TEntity>(
            this IEfCoreRepository repository,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TEntity>> action)
            where TEntity : class
        {
            var dbContext = await repository.GetDbContextAsync();
            return await dbContext.Set<TEntity>().Where(predicate).BatchUpdateAsync(action);
        }

        public static async Task<int> BatchHardDeleteAsync<TEntity>(
            this IEfCoreRepository repository,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var dbContext = await repository.GetDbContextAsync();
            return await dbContext.Set<TEntity>().Where(predicate).BatchDeleteAsync();
        }

        public static async Task BulkInsertAsync<TEntity>(
            this IEfCoreRepository repository,
            IEnumerable<TEntity> entities,
            BulkConfig? bulkConfig = null)
            where TEntity : class
        {
            var dbContext = await repository.GetDbContextAsync();
            await dbContext.BulkInsertAsync(entities.ToList(), bulkConfig);
        }

        public static async Task BulkUpdateAsync<TEntity>(
            this IEfCoreRepository repository,
            IEnumerable<TEntity> entities,
            BulkConfig? bulkConfig = null)
            where TEntity : class
        {
            var dbContext = await repository.GetDbContextAsync();
            await dbContext.BulkUpdateAsync(entities.ToList(), bulkConfig);
        }

        public static async Task BulkDeleteAsync<TEntity>(
            this IEfCoreRepository repository,
            IEnumerable<TEntity> entities,
            BulkConfig? bulkConfig = null)
            where TEntity : class
        {
            var dbContext = await repository.GetDbContextAsync();
            await dbContext.BulkDeleteAsync(entities.ToList(), bulkConfig);
        }

        //public static async Task BulkMergeAsync<TEntity>(
        //    this IEfCoreRepository repository,
        //    IEnumerable<TEntity> entities)
        //    where TEntity : class
        //{
        //    var dbContext = await repository.GetDbContextAsync();
        //    await dbContext.BulkInsertOrUpdateOrDeleteAsync(entities.ToList());
        //}

        //public static async Task BulkSaveChangesAsync(this IEfCoreRepository repository)
        //{
        //    var context = await repository.GetDbContextAsync();
        //    await context.BulkSaveChangesAsync();
        //}

        public static async Task SaveChangesAsync(this IEfCoreRepository repository)
        {
            var context = await repository.GetDbContextAsync();
            await context.SaveChangesAsync();
        }
    }
}
