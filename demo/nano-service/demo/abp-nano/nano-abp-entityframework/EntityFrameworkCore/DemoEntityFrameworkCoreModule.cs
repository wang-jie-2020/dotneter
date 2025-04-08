using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Demo.EntityFrameworkCore
{

    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class DemoEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DemoDbContext>(options =>
            {

            });
        }
    }
}
