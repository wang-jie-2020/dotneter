using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Auditing;

public class AuditLogContributionContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public AuditLogInfo AuditInfo { get; }

    public AuditLogContributionContext(IServiceProvider serviceProvider, AuditLogInfo auditInfo)
    {
        ServiceProvider = serviceProvider;
        AuditInfo = auditInfo;
    }
}
