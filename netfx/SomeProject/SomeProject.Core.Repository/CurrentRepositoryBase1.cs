using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomeProject.Infrastructure.Data.UnitOfWork;
using SomeProject.Infrastructure.Data.Repository;
using SomeProject.Infrastructure.Data;

namespace SomeProject.Core.Repository
{
    //测试泛型主键
    public abstract class CurrentRepositoryBase1<TEntity> : RepositoryBase1<TEntity, long> where TEntity : Entity<long>
    {

    }
}
