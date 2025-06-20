﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Volo.Abp.Uow;
using Yi.Framework.Auditing;
using Yi.System.Monitor.Repositories;

namespace Yi.System.Monitor;

public class AuditingStore : IAuditingStore
{
    public AuditingStore(
        IAuditLogRepository auditLogRepository,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AuditingOptions> options,
        IAuditLogInfoToAuditLogConverter converter)
    {
        AuditLogRepository = auditLogRepository;
        UnitOfWorkManager = unitOfWorkManager;
        Converter = converter;
        Options = options.Value;

        Logger = NullLogger<AuditingStore>.Instance;
    }

    public ILogger<AuditingStore> Logger { get; set; }
    protected IAuditLogRepository AuditLogRepository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected AuditingOptions Options { get; }
    protected IAuditLogInfoToAuditLogConverter Converter { get; }

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
        
        Logger.LogDebug("Yi-请求追踪:" + JsonConvert.SerializeObject(auditInfo, Formatting.Indented, timeConverter));
        using (var uow = UnitOfWorkManager.Begin(true))
        {
            await AuditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));
            await uow.CompleteAsync();
        }
    }
}