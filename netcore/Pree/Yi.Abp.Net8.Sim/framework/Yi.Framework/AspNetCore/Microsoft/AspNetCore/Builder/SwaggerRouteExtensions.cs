using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.AspNetCore.Mvc;

namespace Yi.Framework.AspNetCore.Microsoft.AspNetCore.Builder;

public static class SwaggerRouteExtensions
{
    public static IApplicationBuilder UseYiSwagger(this IApplicationBuilder app, Action<SwaggerUIOptions>? setupAction = null)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            setupAction?.Invoke(c);
        });

        return app;
    }
}