using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkContextRepository.Repository
{

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    /*
        常见的框架的仓储访问基本都是代码生成器输出的模板,继承泛型基类的模式
        例如：
            IRepository<User>
            IUserRepository:IRepository<User>
        
         EntityFramework的特性都忽略掉,这种模式不符合目前的很多面向数据库编程方式
        （1）注入太麻烦
        （2）绝大多数仓储对象都是空的,不会在里面写代码
        （3）通常都会提供更多的泛型方法以方便使用
       
        转换思路，基于EntityFramework时直接将DbContext作为仓储(它本身就有仓储的全部功能)
        不要什么Repository<TEntity>,直接基于DbContext做泛型方法，比如DbContext.Set<TEntity>()
        仓储方法中保存CUD,而R则由IQueryable的DbSet执行
	/*	
	/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    public interface IEfCoreRepository<TDbContext> : IEfCoreRepository
        where TDbContext : DbContext
    {

    }

    public interface IEfCoreRepository : IBasicRepository
    {
        Task<DbContext> GetDbContextAsync();

        Task<DbSet<TEntity>> GetDbSetAsync<TEntity>() where TEntity : class;

        Task<IQueryable<TEntity>> GetQueryableAsync<TEntity>() where TEntity : class;
    }

    public interface IBasicRepository
    {
        Task<TEntity> InsertAsync<TEntity>(TEntity entity, bool saveChanges = false) where TEntity : class;

        Task InsertManyAsync<TEntity>(IEnumerable<TEntity> entities, bool saveChanges = false) where TEntity : class;

        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

        Task UpdateManyAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        Task HardDeleteAsync<TEntity>(TEntity entity) where TEntity : class;

        Task HardDeleteManyAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    }
}