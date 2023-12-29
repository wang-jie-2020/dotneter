using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomeProject.Infrastructure.Common;
using SomeProject.Infrastructure.Common.Helper;
using SomeProject.Infrastructure.Data.Extensions;

namespace SomeProject.Infrastructure.Data.UnitOfWork
{
    //测试泛型主键
    public abstract class UnitOfWorkContextBase1 : IUnitOfWorkContext1
    {
        public void RegisterNew<TEntity, TKey>(TEntity entity) where TEntity : Entity<TKey>
        {
            throw new NotImplementedException();
        }
    }
}
