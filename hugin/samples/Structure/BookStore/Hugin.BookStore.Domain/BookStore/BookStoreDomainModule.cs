using Hugin.BookStore.Stadiums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MultiStadium;

namespace Hugin.BookStore
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule),
        typeof(BookStoreDomainSharedModule)
    )]
    public class BookStoreDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<BookStoreDomainModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BookStoreDomainModule>(validate: false);
            });

            context.Services.Replace(ServiceDescriptor.Transient<IStadiumStore, StadiumStore>());
        }
    }
}
