using Hugin.BookStore;
using Hugin.BookStore.Localization;
using Hugin.BookStore.Permissions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Hugin.BookStoreWeb.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAutoMapperModule),
        typeof(BookStoreHttpApiModule)
        )]
    public class BookStoreWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            //AbpMvcDataAnnotationsLocalizationOptions似乎只在AbpTagHelperLocalizer中引用
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(BookStoreResource), typeof(BookStoreWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(BookStoreWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new BookStoreMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BookStoreWebModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                /*
                 *  本地化中，虽然可以将json文件进行物理拆分，比如下面这种方式
                 *  api块的资源在domainShared中定义/Sample/Localization/Json  web块的资源在web中定义/Localization/Json
                 *  可以满足要求（主要注意文件路径不能重复）
                 *
                 *  但这种操作是否真的是必要的？毕竟模块下的本地化资源应该不太多
                 */
                options.Resources
                    .Get<BookStoreResource>()
                    .AddVirtualJson("/Localization/Json");
            });

            context.Services.AddAutoMapperObjectMapper<BookStoreWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BookStoreWebModule>();
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/BookStore/Books/Index", BookStorePermissions.Book.Default);
                options.Conventions.AuthorizePage("/BookStore/Books/CreateModal", BookStorePermissions.Book.Create);
                options.Conventions.AuthorizePage("/BookStore/Books/EditModal", BookStorePermissions.Book.Edit);
            });
        }
    }
}
