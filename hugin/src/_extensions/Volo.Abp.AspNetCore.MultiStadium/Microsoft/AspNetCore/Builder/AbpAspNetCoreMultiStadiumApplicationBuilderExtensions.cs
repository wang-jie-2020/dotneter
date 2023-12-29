using Volo.Abp.AspNetCore.MultiStadium;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpAspNetCoreMultiStadiumApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiStadium(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MultiStadiumMiddleware>();
        }
    }
}
