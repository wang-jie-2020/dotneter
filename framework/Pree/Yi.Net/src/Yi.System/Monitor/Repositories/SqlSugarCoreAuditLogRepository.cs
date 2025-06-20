﻿using System.Net;
using Yi.Framework.SqlSugarCore;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor.Repositories;

public class SqlSugarCoreAuditLogRepository : SqlSugarRepository<AuditLogEntity, Guid>, IAuditLogRepository
{
    public SqlSugarCoreAuditLogRepository(ISugarDbContextProvider<ISqlSugarDbContext> sugarDbContextProvider) : base(sugarDbContextProvider)
    {
    }

    /// <summary>
    ///     重写插入，支持导航属性
    /// </summary>
    /// <param name="insertObj"></param>
    /// <returns></returns>
    public override async Task<bool> InsertAsync(AuditLogEntity insertObj)
    {
        return await Db.InsertNav(insertObj)
            .Include(z1 => z1.Actions)
            //.Include(z1 => z1.EntityChanges).ThenInclude(z2 => z2.PropertyChanges)
            .ExecuteCommandAsync();
    }

    public virtual async Task<List<AuditLogEntity>> GetListAsync(
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
        bool includeDetails = false)
    {
        var query = await GetListQueryAsync(
            startTime,
            endTime,
            httpMethod,
            url,
            userId,
            userName,
            applicationName,
            clientIpAddress,
            correlationId,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode,
            includeDetails
        );

        var auditLogs = await query
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(AuditLogEntity.ExecutionTime) + " DESC" : sorting)
            .ToPageListAsync(skipCount, maxResultCount);

        return auditLogs;
    }

    public virtual async Task<long> GetCountAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = await GetListQueryAsync(
            startTime,
            endTime,
            httpMethod,
            url,
            userId,
            userName,
            applicationName,
            clientIpAddress,
            correlationId,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode
        );

        var totalCount = await query.CountAsync();

        return totalCount;
    }

    public virtual async Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var result = await DbQueryable
            .Where(a => a.ExecutionTime < endDate.AddDays(1) && a.ExecutionTime > startDate)
            .OrderBy(t => t.ExecutionTime)
            .GroupBy(t => new { t.ExecutionTime.Value.Date })
            .Select(g => new
            {
                Day = SqlFunc.AggregateMin(g.ExecutionTime),
                avgExecutionTime = SqlFunc.AggregateAvg(g.ExecutionDuration)
            })
            .ToListAsync();

        return result.ToDictionary(element => element.Day.Value.ClearTime(), element => (double)element.avgExecutionTime);
    }

    protected virtual async Task<ISugarQueryable<AuditLogEntity>> GetListQueryAsync(
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
        bool includeDetails = false)
    {
        var nHttpStatusCode = (int?)httpStatusCode;
        return DbQueryable
            .WhereIF(startTime.HasValue, auditLog => auditLog.ExecutionTime >= startTime)
            .WhereIF(endTime.HasValue, auditLog => auditLog.ExecutionTime <= endTime)
            .WhereIF(hasException.HasValue && hasException.Value,
                auditLog => auditLog.Exceptions != null && auditLog.Exceptions != "")
            .WhereIF(hasException.HasValue && !hasException.Value,
                auditLog => auditLog.Exceptions == null || auditLog.Exceptions == "")
            .WhereIF(httpMethod != null, auditLog => auditLog.HttpMethod == httpMethod)
            .WhereIF(url != null, auditLog => auditLog.Url != null && auditLog.Url.Contains(url))
            .WhereIF(userId != null, auditLog => auditLog.UserId == userId)
            .WhereIF(userName != null, auditLog => auditLog.UserName == userName)
            .WhereIF(applicationName != null, auditLog => auditLog.ApplicationName == applicationName)
            .WhereIF(clientIpAddress != null,
                auditLog => auditLog.ClientIpAddress != null && auditLog.ClientIpAddress == clientIpAddress)
            .WhereIF(correlationId != null, auditLog => auditLog.CorrelationId == correlationId)
            .WhereIF(httpStatusCode != null && httpStatusCode > 0,
                auditLog => auditLog.HttpStatusCode == nHttpStatusCode)
            .WhereIF(maxExecutionDuration != null && maxExecutionDuration.Value > 0,
                auditLog => auditLog.ExecutionDuration <= maxExecutionDuration)
            .WhereIF(minExecutionDuration != null && minExecutionDuration.Value > 0,
                auditLog => auditLog.ExecutionDuration >= minExecutionDuration);
    }
}