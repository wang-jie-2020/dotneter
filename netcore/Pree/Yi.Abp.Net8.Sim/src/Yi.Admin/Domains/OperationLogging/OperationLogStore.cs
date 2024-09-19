using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;
using Yi.Admin.Domains.OperationLogging.Entities;
using Yi.AspNetCore.Common;

namespace Yi.Admin.Domains.OperationLogging;

public class OperationLogStore : IOperationLogStore
{
    private readonly ILogger<OperationLogStore> _logger;
    private readonly IRepository<OperationLogEntity> _repository;

    public OperationLogStore(ILogger<OperationLogStore> logger, IRepository<OperationLogEntity> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task SaveAsync(OperationLogInfo operationLogInfo)
    {
        var entity = new OperationLogEntity
        {
            Title = operationLogInfo.Title,
            OperType = operationLogInfo.OperationLog,
            OperUser = operationLogInfo.Operator,
            Method = operationLogInfo.Method,
            RequestMethod = operationLogInfo.RequestMethod,
            RequestParam = operationLogInfo.RequestParam,
            RequestResult = operationLogInfo.Result,
            ExecutionTime = operationLogInfo.ExecutionTime
        };

        await _repository.InsertAsync(entity);
    }
}