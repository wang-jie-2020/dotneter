using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public static class StadiumResolveContextExtensions
    {
        public static AbpAspNetCoreMultiStadiumOptions GetAbpAspNetCoreMultiStadiumOptions(this IStadiumResolveContext context)
        {
            return context.ServiceProvider.GetRequiredService<IOptionsSnapshot<AbpAspNetCoreMultiStadiumOptions>>().Value;
        }
    }
}