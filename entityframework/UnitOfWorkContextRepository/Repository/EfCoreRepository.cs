using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkContextRepository.Repository
{
    public class EfCoreRepository<TDbContext> : IEfCoreRepository<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public virtual async Task<DbContext> GetDbContextAsync()
        {
            return await _dbContextProvider.GetDbContextAsync() as DbContext ?? throw new Exception("DbContext Null");
        }

        public virtual async Task<DbSet<TEntity>> GetDbSetAsync<TEntity>()
            where TEntity : class
        {
            return (await GetDbContextAsync()).Set<TEntity>();
        }

        public virtual async Task<IQueryable<TEntity>> GetQueryableAsync<TEntity>()
            where TEntity : class
        {
            return (await GetDbSetAsync<TEntity>()).AsQueryable();
        }

        public virtual async Task<TEntity> InsertAsync<TEntity>(TEntity entity, bool saveChanges = false)
            where TEntity : class
        {
            var dbContext = await GetDbContextAsync();
            var savedEntity = (await dbContext.Set<TEntity>().AddAsync(entity)).Entity;

            if (saveChanges)
            {
               await dbContext.SaveChangesAsync();
            }

            return savedEntity;
        }

        public virtual async Task InsertManyAsync<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false)
            where TEntity : class
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.Set<TEntity>().AddRangeAsync(entities);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public virtual async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            var dbContext = await GetDbContextAsync();
            dbContext.Attach(entity);
            var updatedEntity = dbContext.Update(entity).Entity;

            return updatedEntity;
        }

        public virtual async Task UpdateManyAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            var dbContext = await GetDbContextAsync();

            dbContext.Set<TEntity>().UpdateRange(entities);
        }

        public virtual async Task HardDeleteAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            var dbContext = await GetDbContextAsync();

            dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual async Task HardDeleteManyAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            var dbContext = await GetDbContextAsync();

            dbContext.RemoveRange(entities);
        }
    }
}