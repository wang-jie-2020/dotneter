using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Hugin.BookStore.Daos
{
    public interface IBookDaoService : ITransientDependency
    {
        IQueryable<BookDao> QueryBook();
    }
}
