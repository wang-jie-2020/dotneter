using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Hugin.BookStore.Daos
{
    public interface IBookShopDaoService : ITransientDependency
    {
        IQueryable<BookShopDao> QueryBookShop();
    }
}
