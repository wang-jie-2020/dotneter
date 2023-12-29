using Hugin.Mvc.Conventions;
using Hugin.Mvc.DependencyInjection;
using Hugin.Mvc.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Modularity;

namespace Hugin.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class HuginMvcModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new HuginDefaultConventionalRegistrar());
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpRemoteServiceApiDescriptionProviderOptions>(options =>
            {
                options.SupportedResponseTypes.Clear(); //约定即可
            });

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpExceptionHandlingOptions>(options =>
                {
                    options.SendExceptionsDetailsToClients = true;
                });
            }

            Configure<MvcOptions>(options =>
            {
                options.Filters.Remove(new AbpAutoValidateAntiforgeryTokenAttribute()); //影响带IFormFile的请求，而且也不是很合适
            });

            //重写AbpServiceConvention
            context.Services.Replace(ServiceDescriptor.Transient<IAbpServiceConvention, HuginServiceConvention>());

            //替换Abp的错误处理
            context.Services.Replace(ServiceDescriptor.Transient<AbpExceptionFilter, HuginExceptionFilter>());

            ////增加结果处理    
            //context.Services.AddTransient<HuginResultFilter>();  //因abp的httpClient在设计时未考虑到可能出现的封装，故不再封装
            //Configure<MvcOptions>(options =>
            //{
            //    options.Filters.AddService<HuginResultFilter>();
            //});

            //Metadata处理
            //context.Services.Replace(ServiceDescriptor.Singleton<IModelMetadataProvider, HuginModelMetadataProvider>());
            //Configure<MvcOptions>(options =>
            //{
            //    options.ModelMetadataDetailsProviders.Add(new HuginValidationMetadataProvider());
            //});

            //context.Services.Replace(ServiceDescriptor.Singleton<IValidationAttributeAdapterProvider, HuginValidationAttributeAdapterProvider>());
        }
    }
}
