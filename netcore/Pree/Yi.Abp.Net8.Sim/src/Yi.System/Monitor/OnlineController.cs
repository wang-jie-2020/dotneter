﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.System.Hubs;

namespace Yi.System.Monitor;

[ApiController]
[Route("api/app/online")]
public class OnlineController : AbpController
{
    private readonly ILogger<OnlineController> _logger;
    private readonly IHubContext<MainHub> _hub;

    public OnlineController(ILogger<OnlineController> logger, IHubContext<MainHub> hub)
    {
        _logger = logger;
        _hub = hub;
    }

    [HttpGet]
    public Task<PagedResultDto<OnlineUserModel>> GetListAsync([FromQuery] OnlineUserModel online)
    {
        var data = MainHub.clientUsers;
        var dataWhere = data.AsEnumerable();

        if (!string.IsNullOrEmpty(online.Ipaddr))
        {
            dataWhere = dataWhere.Where(u => u.Ipaddr!.Contains(online.Ipaddr));
        }

        if (!string.IsNullOrEmpty(online.UserName))
        {
            dataWhere = dataWhere.Where(u => u.UserName!.Contains(online.UserName));
        }

        return Task.FromResult(new PagedResultDto<OnlineUserModel> { TotalCount = data.Count, Items = dataWhere.ToList() });
    }
    
    /// <summary>
    ///     强制退出用户
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    [HttpDelete("{connectionId}")]
    public async Task<bool> ForceOut([FromRoute] string connectionId)
    {
        if (MainHub.clientUsers.Exists(u => u.ConnectionId == connectionId))
        {
            //前端接受到这个事件后，触发前端自动退出
            await _hub.Clients.Client(connectionId).SendAsync("forceOut", "你已被强制退出！");
            return true;
        }

        return false;
    }
}