using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Yi.AspNetCore.Auditing;

namespace Microsoft.AspNetCore.Builder;

public static class AbpApplicationBuilderExtensions
{
    public async static Task InitializeApplicationAsync([NotNull] this IApplicationBuilder app)
    {
        Check.NotNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStopping.Register(() =>
        {
            AsyncHelper.RunSync(() => application.ShutdownAsync());
        });

        applicationLifetime.ApplicationStopped.Register(() =>
        {
            application.Dispose();
        });

        await application.InitializeAsync(app.ApplicationServices);
    }

    public static void InitializeApplication([NotNull] this IApplicationBuilder app)
    {
        Check.NotNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStopping.Register(() =>
        {
            application.Shutdown();
        });

        applicationLifetime.ApplicationStopped.Register(() =>
        {
            application.Dispose();
        });

        application.Initialize(app.ApplicationServices);
    }

    public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
    {
        return app
            .UseMiddleware<AuditingMiddleware>();
    }
}
