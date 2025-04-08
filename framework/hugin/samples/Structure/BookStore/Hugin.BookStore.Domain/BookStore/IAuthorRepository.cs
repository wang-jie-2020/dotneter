using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Hugin.BookStore
{
    /*
     * 在abp的设计中，仓储接口的设计是在Domain中的，它在EFCore中实现
     * Domain包含了Entity、DomainBusiness、IRepository太过重了，让其更专注业务是不是更好？
     * 从正常意义上理解的仓储接口定义应当考虑再新建类库，比如IDataAccess，但实际需求较少，就和Entity放在一起试试
     */
    public interface IAuthorRepository : IRepository<Author, Guid>
    {
        Task<Author> FindByNameAsync(string name);

        Task<List<Author>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null);
    }
}
