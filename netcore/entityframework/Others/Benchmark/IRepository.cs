using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace RepositoryModule
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task GetQueryableAsync<TEntity>();
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public async Task GetQueryableAsync<TEntity>()
        {
            await Task.Delay(1);
            Console.WriteLine(typeof(TEntity).FullName);
        }
    }
}