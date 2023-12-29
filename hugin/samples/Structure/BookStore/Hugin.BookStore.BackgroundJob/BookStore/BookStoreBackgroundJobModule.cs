using Volo.Abp.Modularity;

namespace Hugin.BookStore
{
    [DependsOn(
        typeof(BookStoreDomainModule)
    )]
    public class BookStoreBackgroundJobModule : AbpModule
    {
    }
}
