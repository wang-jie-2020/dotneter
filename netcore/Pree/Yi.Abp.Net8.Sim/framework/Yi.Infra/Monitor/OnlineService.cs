using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Infra.Rbac.IServices;
using Yi.Infra.Rbac.Model;
using Yi.Infra.Rbac.SignalRHubs;

namespace Yi.Infra.Monitor;

public class OnlineService : ApplicationService, IOnlineService
{
    private readonly IHubContext<OnlineHub> _hub;
    private ILogger<OnlineService> _logger;

    public OnlineService(ILogger<OnlineService> logger, IHubContext<OnlineHub> hub)
    {
        _logger = logger;
        _hub = hub;
    }

    /// <summary>
    ///     动态条件获取当前在线用户
    /// </summary>
    /// <param name="online"></param>
    /// <returns></returns>
    public Task<PagedResultDto<OnlineUserModel>> GetListAsync([FromQuery] OnlineUserModel online)
    {
        var data = OnlineHub.clientUsers;
        var dataWhere = data.AsEnumerable();

        if (!string.IsNullOrEmpty(online.Ipaddr)) dataWhere = dataWhere.Where(u => u.Ipaddr!.Contains(online.Ipaddr));
        if (!string.IsNullOrEmpty(online.UserName))
            dataWhere = dataWhere.Where(u => u.UserName!.Contains(online.UserName));
        return Task.FromResult(new PagedResultDto<OnlineUserModel>
            { TotalCount = data.Count, Items = dataWhere.ToList() });
    }


    /// <summary>
    ///     强制退出用户
    /// </summary>
    /// <param name="connnectionId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("online/{connnectionId}")]
    public async Task<bool> ForceOut(string connnectionId)
    {
        if (OnlineHub.clientUsers.Exists(u => u.ConnnectionId == connnectionId))
        {
            //前端接受到这个事件后，触发前端自动退出
            await _hub.Clients.Client(connnectionId).SendAsync("forceOut", "你已被强制退出！");
            return true;
        }

        return false;
    }
}