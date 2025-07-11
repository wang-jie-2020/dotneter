using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Auditing;

[Dependency(TryRegister = true)]
public class NullAuditingStore : IAuditingStore, ISingletonDependency
{
    public ILogger<NullAuditingStore> Logger { get; set; }

    public NullAuditingStore()
    {
        Logger = NullLogger<NullAuditingStore>.Instance;
    }

    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        Logger.LogInformation(auditInfo.ToString());
        return Task.FromResult(0);
    }
}
