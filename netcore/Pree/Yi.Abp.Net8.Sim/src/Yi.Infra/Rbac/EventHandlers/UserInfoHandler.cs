using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Yi.Abp.Infra.Rbac.Etos;
using Yi.Abp.Infra.Rbac.Managers;

namespace Yi.Abp.Infra.Rbac.EventHandlers
{
    public class UserInfoHandler : ILocalEventHandler<UserRoleMenuQueryEventArgs>, ITransientDependency
    {
        private UserManager _userManager;
        public UserInfoHandler(UserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task HandleEventAsync(UserRoleMenuQueryEventArgs eventData)
        {
            //数据库查询方式
            var result = await _userManager.GetInfoListAsync(eventData.UserIds);
            eventData.Result = result;
        }
    }
}
