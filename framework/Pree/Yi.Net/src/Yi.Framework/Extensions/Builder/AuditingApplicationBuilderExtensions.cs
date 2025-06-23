using Microsoft.AspNetCore.Builder;
using Yi.Framework.Auditing;

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
