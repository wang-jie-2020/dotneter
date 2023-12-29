using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SomeProject.Infrastructure.Common;
using SomeProject.Infrastructure.Data;

namespace SomeProject.Infrastructure.Data.Repository
{
    //测试泛型主键
    public interface IRepository1<TEntity, TKey> where TEntity : Entity<TKey>
    {

    }
}
