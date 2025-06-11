using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Yi.System.Domains;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor;

public class LoginEventHandler : ILocalEventHandler<LoginEventArgs>, ITransientDependency
{
    private readonly ILogger<LoginEventHandler> _logger;
    private readonly ISqlSugarRepository<LoginLogEntity> _loginLogRepository;

    public LoginEventHandler(ILogger<LoginEventHandler> logger, ISqlSugarRepository<LoginLogEntity> loginLogRepository)
    {
        _logger = logger;
        _loginLogRepository = loginLogRepository;
    }

    public async Task HandleEventAsync(LoginEventArgs eventData)
    {
        _logger.LogInformation($"用户【{eventData.UserId}:{eventData.UserName}】登入系统");
        var loginLogEntity = eventData.Adapt<LoginLogEntity>();
        loginLogEntity.LogMsg = eventData.UserName + "登录系统";
        loginLogEntity.LoginUser = eventData.UserName;
        loginLogEntity.CreatorId = eventData.UserId;
        await _loginLogRepository.InsertAsync(loginLogEntity);
    }
}