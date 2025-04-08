using Hugin.BookStore;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Hugin
{
    [DependsOn(
        typeof(BookStoreHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class BookStoreHttpApiClientTestModule : AbpModule
    {

    }
}
