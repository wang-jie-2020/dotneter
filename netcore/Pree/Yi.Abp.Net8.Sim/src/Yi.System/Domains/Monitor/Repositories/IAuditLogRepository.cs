﻿using System.Net;
using Volo.Abp.Auditing;
using Yi.System.Domains.Monitor.Entities;

namespace Yi.System.Domains.Monitor.Repositories;

public interface IAuditLogRepository : ISqlSugarRepository<AuditLogEntity, Guid>
{
    Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        DateTime startDate, 
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null, 
        string? httpMethod = null,
        string? url = null, 
        Guid? userId = null,
        string? userName = null, 
        string? applicationName = null,
        string? clientIpAddress = null, 
        string? correlationId = null, 
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null, 
        bool? hasException = null, 
        HttpStatusCode? httpStatusCode = null,
        CancellationToken cancellationToken = default);

    Task<EntityChangeEntity> GetEntityChange(Guid entityChangeId, CancellationToken cancellationToken = default);

    Task<long> GetEntityChangeCountAsync(
        Guid? auditLogId = null, 
        DateTime? startTime = null, 
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string? entityId = null,
        string? entityTypeFullName = null,
        CancellationToken cancellationToken = default);

    Task<List<EntityChangeEntity>> GetEntityChangeListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0, 
        Guid? auditLogId = null, 
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string? entityId = null, 
        string? entityTypeFullName = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<EntityChangeWithUsername>> GetEntityChangesWithUsernameAsync(
        string entityId,
        string entityTypeFullName,
        CancellationToken cancellationToken = default);

    Task<EntityChangeWithUsername> GetEntityChangeWithUsernameAsync(Guid entityChangeId);

    Task<List<AuditLogEntity>> GetListAsync(
        string? sorting = null, 
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null, 
        DateTime? endTime = null,
        string? httpMethod = null,
        string? url = null,
        Guid? userId = null, 
        string? userName = null,
        string? applicationName = null,
        string? clientIpAddress = null,
        string? correlationId = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null, 
        HttpStatusCode? httpStatusCode = null, 
        bool includeDetails = false);
}