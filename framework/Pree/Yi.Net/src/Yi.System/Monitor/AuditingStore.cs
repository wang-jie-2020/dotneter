using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Volo.Abp.Uow;
using Yi.Framework.Auditing;
using Yi.Framework.Utils;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor;

public class AuditingStore : IAuditingStore
{
    public ILogger<AuditingStore> Logger { get; set; }
    protected ISqlSugarRepository<AuditLogEntity> AuditLogRepository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected AuditingOptions Options { get; }
    
    public AuditingStore(
        ISqlSugarRepository<AuditLogEntity> auditLogRepository,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AuditingOptions> options)
    {
        AuditLogRepository = auditLogRepository;
        UnitOfWorkManager = unitOfWorkManager;
        Options = options.Value;

        Logger = NullLogger<AuditingStore>.Instance;
    }

    public virtual async Task SaveAsync(AuditLogInfo auditInfo)
    {
        try
        {
            await SaveLogAsync(auditInfo);
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditInfo);
            Logger.LogException(ex, LogLevel.Error);
        }
    }

    protected virtual async Task SaveLogAsync(AuditLogInfo auditInfo)
    {
        var timeConverter = new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
        };
        
        Logger.LogTrace("Yi-请求追踪:" + JsonConvert.SerializeObject(auditInfo, Formatting.Indented, timeConverter));
        using (var uow = UnitOfWorkManager.Begin(true))
        {
            var auditLogId = SequentialGuidGenerator.Create();

            var actions = auditInfo.Actions?
                .Select(auditLogActionInfo => new AuditLogActionEntity(SequentialGuidGenerator.Create(), auditLogId,
                    auditLogActionInfo, auditInfo.TenantId))
                .ToList() ?? new List<AuditLogActionEntity>();

            var comments = auditInfo
                .Comments?
                .JoinAsString(Environment.NewLine);

            var auditLog = new AuditLogEntity(
                auditLogId,
                auditInfo.ApplicationName,
                auditInfo.TenantId,
                auditInfo.TenantName,
                auditInfo.UserId,
                auditInfo.UserName,
                auditInfo.ExecutionTime,
                auditInfo.ExecutionDuration,
                auditInfo.ClientIpAddress,
                auditInfo.ClientName,
                auditInfo.ClientId,
                auditInfo.CorrelationId,
                auditInfo.BrowserInfo,
                auditInfo.HttpMethod,
                auditInfo.Url,
                auditInfo.HttpStatusCode,
                actions,
                string.Empty,
                comments
            );
            
            await AuditLogRepository.Context.InsertNav(auditLog)
                .Include(z1 => z1.Actions)
                //.Include(z1 => z1.EntityChanges).ThenInclude(z2 => z2.PropertyChanges)
                .ExecuteCommandAsync();
            
            await uow.CompleteAsync();
        }
    }
}