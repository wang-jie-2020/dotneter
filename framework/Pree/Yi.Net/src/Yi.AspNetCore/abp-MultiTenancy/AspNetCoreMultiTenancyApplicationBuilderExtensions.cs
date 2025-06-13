using Microsoft.AspNetCore.Builder;

namespace Yi.AspNetCore.MultiTenancy;

public static class AspNetCoreMultiTenancyApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MultiTenancyMiddleware>();
    }
}
