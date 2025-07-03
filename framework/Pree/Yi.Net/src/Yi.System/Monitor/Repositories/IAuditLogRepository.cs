using System.Net;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor.Repositories;

public interface IAuditLogRepository : ISqlSugarRepository<AuditLogEntity>
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