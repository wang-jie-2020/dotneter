using System.Threading.Tasks;
using Hugin.Identity.Users;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.TenantManagement;

namespace Hugin.BookStore.Controllers
{
    [Route("api/remote")]
    public class RemoteController : AbpController
    {
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IIdentityUserLookupAppService _identityUserLookupAppService;
        private readonly IIdentityRoleAppService _identityRoleAppService;
        private readonly IProfileAppService _profileAppService;
        private readonly ITenantAppService _tenantAppService;
        private readonly IUserExtendAppService _userAppService;

        public RemoteController(IIdentityUserAppService identityUserAppService,
           IIdentityUserLookupAppService identityUserLookupAppService,
           IIdentityRoleAppService identityRoleAppService,
           IProfileAppService profileAppService,
           ITenantAppService tenantAppService,
           IUserExtendAppService userAppService)
        {
            _identityUserAppService = identityUserAppService;
            _identityUserLookupAppService = identityUserLookupAppService;
            _identityRoleAppService = identityRoleAppService;
            _profileAppService = profileAppService;
            _tenantAppService = tenantAppService;
            _userAppService = userAppService;
        }

        [HttpGet]
        [Route("get-user")]
        public virtual async Task<object> GetUsers()
        {
            return await _identityUserAppService.GetListAsync(new GetIdentityUsersInput());
        }

        [HttpGet]
        [Route("get-tenant")]
        public virtual async Task<object> GetTenant()
        {
            return await _tenantAppService.GetListAsync(new GetTenantsInput());
        }
    }
}
