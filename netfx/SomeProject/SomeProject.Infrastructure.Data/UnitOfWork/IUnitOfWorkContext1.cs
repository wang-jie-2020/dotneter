using SomeProject.Infrastructure.Common;
using SomeProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeProject.Infrastructure.Data.UnitOfWork
{
    //测试泛型主键
    public interface IUnitOfWorkContext1 
    {
        void RegisterNew<TEntity, TKey>(TEntity entity) where TEntity : Entity<TKey>;
    }
}
