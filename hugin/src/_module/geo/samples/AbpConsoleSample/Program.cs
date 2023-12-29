using System;
using System.Diagnostics;
using Abp;
using Abp.AspNetCore;
using Abp.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedisGEO.Abp;
using RedisGEO.Core;

namespace AbpConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bootstrapper = AbpBootstrapper.Create<AbpConsoleSampleModule>())
            {
                bootstrapper.Initialize();

                var options = bootstrapper.IocManager.Resolve<GeoConfig>();
                var service = bootstrapper.IocManager.Resolve<IAbpGeoService>();

                Debug.Assert(options == null);
                Debug.Assert(service == null);
            }
        }
    }
}
