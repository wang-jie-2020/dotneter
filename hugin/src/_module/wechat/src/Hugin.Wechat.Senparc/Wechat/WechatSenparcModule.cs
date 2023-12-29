using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.Cache.CsRedis;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.WxOpen;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Wechat
{
    public class WechatSenparcModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddSenparcWeixinServices(configuration);
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
            var env = context.ServiceProvider.GetRequiredService<IHostEnvironment>();

            var senparcSetting = context.ServiceProvider.GetRequiredService<IOptions<SenparcSetting>>().Value;
            var senparcWeixinSetting = context.ServiceProvider.GetRequiredService<IOptions<SenparcWeixinSetting>>().Value;

            app.UseSenparcGlobal(env, senparcSetting, globalRegister =>
                {
                    Senparc.CO2NET.Cache.CsRedis.Register.SetConfigurationOption(senparcSetting.Cache_Redis_Configuration);
                    Senparc.CO2NET.Cache.CsRedis.Register.UseKeyValueRedisNow();
                }, true)
                .UseSenparcWeixin(senparcWeixinSetting, weixinRegister =>
                {
                    weixinRegister.UseSenparcWeixinCacheCsRedis();
                    weixinRegister
                        .RegisterMpAccount(senparcWeixinSetting, "公众号")
                        .RegisterWxOpenAccount(senparcWeixinSetting, "小程序")
                        .RegisterTenpayV3(senparcWeixinSetting, "公众号");
                });
        }
    }
}
