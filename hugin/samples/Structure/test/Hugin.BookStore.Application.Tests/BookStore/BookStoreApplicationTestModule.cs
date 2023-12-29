using Volo.Abp.Modularity;

namespace Hugin.BookStore
{
    [DependsOn(
        typeof(BookStoreApplicationModule),
        typeof(BookStoreDomainTestModule)
    )]
    public class BookStoreApplicationTestModule : AbpModule
    {

    }
}
