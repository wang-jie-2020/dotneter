using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.SignalR;
using Yi.Infra.Rbac.Account;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Hubs;

[HubRoute("/hub/main")]
//开放不需要授权
//[Authorize]
public class MainHub : AbpHub
{
    public static readonly List<OnlineUserModel> clientUsers = new();
    private static readonly object objLock = new();

    private readonly HttpContext? _httpContext;

    public MainHub(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor?.HttpContext;
    }

    private ILogger<MainHub> _logger => LoggerFactory.CreateLogger<MainHub>();

    /// <summary>
    ///     成功连接
    /// </summary>
    /// <returns></returns>
    public override Task OnConnectedAsync()
    {
        lock (objLock)
        {
            var name = CurrentUser.UserName;
            var loginUser = new LoginLogAggregateRoot().GetInfoByHttpContext(_httpContext);

            OnlineUserModel user = new(Context.ConnectionId)
            {
                Browser = loginUser?.Browser,
                LoginLocation = loginUser?.LoginLocation,
                Ipaddr = loginUser?.LoginIp,
                LoginTime = DateTime.Now,
                Os = loginUser?.Os,
                UserName = name ?? "Null",
                UserId = CurrentUser.Id ?? Guid.Empty
            };

            //已登录
            if (CurrentUser.Id is not null)
            {
                //先移除之前的用户id，一个用户只能一个
                clientUsers.RemoveAll(u => u.UserId == CurrentUser.Id);
                _logger.LogInformation($"{DateTime.Now}：{name},{Context.ConnectionId}连接服务端success，当前已连接{clientUsers.Count}个");
            }

            //全部移除之后，再进行添加
            clientUsers.RemoveAll(u => u.ConnectionId == Context.ConnectionId);
            clientUsers.Add(user);
            
            //当有人加入，向全部客户端发送当前总数
            Clients.All.SendAsync("onlineNum", clientUsers.Count);
        }

        return base.OnConnectedAsync();
    }


    /// <summary>
    ///     断开连接
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override Task OnDisconnectedAsync(Exception exception)
    {
        lock (objLock)
        {
            //已登录
            if (CurrentUser.Id is not null)
            {
                clientUsers.RemoveAll(u => u.UserId == CurrentUser.Id);
                _logger.LogInformation($"用户{CurrentUser?.UserName}离开了，当前已连接{clientUsers.Count}个");
            }

            clientUsers.RemoveAll(u => u.ConnectionId == Context.ConnectionId);
            Clients.All.SendAsync("onlineNum", clientUsers.Count);
        }

        return base.OnDisconnectedAsync(exception);
    }
}