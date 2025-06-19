using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Extensions.DependencyInjection;

public static class ServiceProviderAccessorExtensions
{
    public static HttpContext? GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor)
    {
        return serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
    }
}
