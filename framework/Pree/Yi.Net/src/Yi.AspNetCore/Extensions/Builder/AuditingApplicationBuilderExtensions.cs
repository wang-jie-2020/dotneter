using Microsoft.AspNetCore.Builder;
using Yi.AspNetCore.Auditing;

namespace Yi.Framework.Extensions.Builder
{
    public static class AuditingApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuditingMiddleware>();
        }
    }
}
