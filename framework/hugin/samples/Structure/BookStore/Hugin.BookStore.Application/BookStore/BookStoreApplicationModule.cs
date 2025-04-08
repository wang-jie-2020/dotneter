using Hugin.Infrastructure.Exporting;
using Hugin.Infrastructure.Exporting.Magicodes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Hugin.BookStore
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(BookStoreDomainModule),
        typeof(BookStoreApplicationContractsModule),
        typeof(BookStoreBackgroundJobModule),
        typeof(BookStoreCapModule)
        )]
    public class BookStoreApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<BookStoreApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                /*
                 *  validate 检查源、目标间的属性是否一致，通常不会设置true
                 *  若必要则单独增加options.AddProfile < ...> (validate: true or false);
                 */
                options.AddMaps<BookStoreApplicationModule>(validate: false);
            });

            context.Services.AddTransient<IExcelExporting, ExcelExporting>();
        }
    }
}