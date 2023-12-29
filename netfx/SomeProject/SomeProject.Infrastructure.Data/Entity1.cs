using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeProject.Infrastructure.Data
{
    //测试泛型主键
    public abstract class Entity<TKey> 
    {
        public TKey Id { get; set; }
    }
}
