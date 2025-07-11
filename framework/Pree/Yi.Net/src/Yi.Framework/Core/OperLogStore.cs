using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Mvc.OperLogging;
using Yi.Framework.Core.Entities;
using Yi.Framework.SqlSugarCore.Repositories;

namespace Yi.Framework.Core;

public class OperLogStore : IOperLogStore, ISingletonDependency
{
    private readonly ILogger<OperLogStore> _logger;
    private readonly ISqlSugarRepository<OperLogEntity> _repository;

    public OperLogStore(ILogger<OperLogStore> logger, ISqlSugarRepository<OperLogEntity> repository)
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