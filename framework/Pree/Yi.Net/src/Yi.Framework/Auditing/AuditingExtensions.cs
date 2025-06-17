using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
