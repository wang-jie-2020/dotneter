using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Auditing;

public class AspNetCoreAuditLogContributor : AuditLogContributor, ITransientDependency
{
    public ILogger<AspNetCoreAuditLogContributor> Logger { get; set; }

    public AspNetCoreAuditLogContributor()
    {
        Logger = NullLogger<AspNetCoreAuditLogContributor>.Instance;
    }

    public override void PreContribute(AuditLogContributionContext context)
    {
        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null)
        {
            return;
        }

        if (httpContext.WebSockets.IsWebSocketRequest)
        {
            return;
        }

        if (context.AuditInfo.HttpMethod == null)
        {
            context.AuditInfo.HttpMethod = httpContext.Request.Method;
        }

        if (context.AuditInfo.Url == null)
        {
            context.AuditInfo.Url = BuildUrl(httpContext);
        }
    }

    public override void PostContribute(AuditLogContributionContext context)
    {
        if (context.AuditInfo.HttpStatusCode != null)
        {
            return;
        }

        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null)
        {
            return;
        }

        if (context.AuditInfo.Exceptions.Any())
        {
            foreach (var auditInfoException in context.AuditInfo.Exceptions)
            {
                context.AuditInfo.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            }

            if (context.AuditInfo.HttpStatusCode != null)
            {
                return;
            }
        }

        context.AuditInfo.HttpStatusCode = httpContext.Response.StatusCode;
    }

    protected virtual string BuildUrl(HttpContext httpContext)
    {
        //TODO: Add options to include/exclude query, schema and host

        var uriBuilder = new UriBuilder
        {
            Scheme = httpContext.Request.Scheme,
            Host = httpContext.Request.Host.Host,
            Path = httpContext.Request.Path.ToString(),
            Query = httpContext.Request.QueryString.ToString()
        };

        return uriBuilder.Uri.AbsolutePath;
    }
}
