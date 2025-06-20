using Microsoft.AspNetCore.Builder;
using Yi.AspNetCore.MultiTenancy;

namespace Yi.AspNetCore.Extensions.Builder;

public static class MultiTenancyApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MultiTenancyMiddleware>();
    }
}
