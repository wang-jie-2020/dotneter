using Microsoft.AspNetCore.Builder;
using Yi.AspNetCore.Auditing;

namespace Yi.Framework.Auditing
{
    public static class AuditingExtensions
    {
        public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AuditingMiddleware>();
        }
    }
}
