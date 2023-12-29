using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Hugin.BookStore
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(BookStoreDomainSharedModule)
        )]
    public class BookStoreApplicationContractsModule : AbpModule
    {

    }
}
