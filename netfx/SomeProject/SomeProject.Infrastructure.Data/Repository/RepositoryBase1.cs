using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SomeProject.Infrastructure.Common;
using SomeProject.Infrastructure.Common.Exceptions;
using SomeProject.Infrastructure.Common.Helper;
using SomeProject.Infrastructure.Data.UnitOfWork;

namespace SomeProject.Infrastructure.Data.Repository
{
    //测试泛型主键
    public abstract class RepositoryBase1<TEntity, TKey> : IRepository1<TEntity, TKey> where TEntity : Entity<TKey>
    {

    }
}
