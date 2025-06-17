using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Yi.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IWebHostEnvironment GetHostingEnvironment(this IServiceCollection services)
    {
        var hostingEnvironment = services.GetSingletonInstanceOrNull<IWebHostEnvironment>();

        return hostingEnvironment!;
    }
}
