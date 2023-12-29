using System;
using System.Collections.Generic;
using System.Text;
using Abp.Modules;
using Abp.Reflection.Extensions;
using RedisGEO.Abp;

namespace AbpConsoleSample
{
    [DependsOn(typeof(AbpRedisGeoModule))]
    public class AbpConsoleSampleModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.UseAbpGeoRedis(options =>
            {
                options.ConnectionString = "127.0.0.1";
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpConsoleSampleModule).GetAssembly());
        }
    }
}
