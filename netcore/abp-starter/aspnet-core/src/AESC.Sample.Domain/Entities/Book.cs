using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESC.Sample.Entities
{
    public class Book : FullAuditedAggregateRoot<long>
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }
    }
}
