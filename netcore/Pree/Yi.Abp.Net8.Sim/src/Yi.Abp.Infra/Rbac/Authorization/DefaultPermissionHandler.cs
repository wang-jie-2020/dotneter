using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;
using Yi.Abp.Infra.Rbac.Consts;
using Yi.Framework.Core.Extensions;

namespace Yi.Abp.Infra.Rbac.Authorization
{
    public class DefaultPermissionHandler : IPermissionHandler, ITransientDependency
    {
        private ICurrentUser _currentUser { get; set; }
        private IHttpContextAccessor _httpContextAccessor;

        public DefaultPermissionHandler(ICurrentUser currentUser, IHttpContextAccessor httpContextAccessor)
        {
            _currentUser = currentUser;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool IsPass(string permission)
        {
            var permissions = _httpContextAccessor.HttpContext.GetUserPermissions(TokenTypeConst.Permission);
            if (permissions is not null)
            {
                if (permissions.Contains("*:*:*"))
                {
                    return true;
                }

                return permissions.Contains(permission);

            }

            return false;
        }
    }
}