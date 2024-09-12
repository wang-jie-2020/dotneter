﻿using Mapster;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;

namespace Yi.Infra.Rbac.Account;

public class LoginEventHandler : ILocalEventHandler<LoginEventArgs>,
    ITransientDependency
{
    private readonly ILogger<LoginEventHandler> _logger;
    private readonly IRepository<LoginLogAggregateRoot> _loginLogRepository;

    public LoginEventHandler(ILogger<LoginEventHandler> logger, IRepository<LoginLogAggregateRoot> loginLogRepository)
    {
        _logger = logger;
        _loginLogRepository = loginLogRepository;
    }

    public async Task HandleEventAsync(LoginEventArgs eventData)
    {
        _logger.LogInformation($"用户【{eventData.UserId}:{eventData.UserName}】登入系统");
        var loginLogEntity = eventData.Adapt<LoginLogAggregateRoot>();
        loginLogEntity.LogMsg = eventData.UserName + "登录系统";
        loginLogEntity.LoginUser = eventData.UserName;
        loginLogEntity.CreatorId = eventData.UserId;
        //异步插入
        await _loginLogRepository.InsertAsync(loginLogEntity);
    }
}