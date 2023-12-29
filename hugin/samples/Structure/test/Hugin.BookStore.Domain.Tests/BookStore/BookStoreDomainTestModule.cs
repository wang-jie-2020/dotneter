using Hugin.BookStore.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Hugin.BookStore
{
    [DependsOn(
        typeof(BookStoreEntityFrameworkCoreTestModule)
        )]
    public class BookStoreDomainTestModule : AbpModule
    {

    }
}
