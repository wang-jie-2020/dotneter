using Abp.Modules;
using Abp.Reflection.Extensions;
using RedisGEO.Core;

namespace RedisGEO.Abp
{
    public class AbpRedisGeoModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<GeoConfig>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpRedisGeoModule).GetAssembly());
        }
    }
}
