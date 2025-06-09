using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;
using Yi.AspNetCore.Core.Loggings;
using Yi.Sys.Domains.Monitor.Entities;

namespace Yi.Sys.Domains.Monitor;

public class OperLogStore : IOperLogStore
{
    private readonly ILogger<OperLogStore> _logger;
    private readonly IRepository<OperLogEntity> _repository;

    public OperLogStore(ILogger<OperLogStore> logger, IRepository<OperLogEntity> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task SaveAsync(OperLogInfo operLogInfo)
    {
        var entity = new OperLogEntity
        {
            Title = operLogInfo.Title,
            OperType = operLogInfo.OperLog,
            OperUser = operLogInfo.Operator,
            Method = operLogInfo.Method,
            RequestMethod = operLogInfo.RequestMethod,
            RequestParam = operLogInfo.RequestParam,
            RequestResult = operLogInfo.Result,
            ExecutionTime = operLogInfo.ExecutionTime
        };

        await _repository.InsertAsync(entity);
    }
}