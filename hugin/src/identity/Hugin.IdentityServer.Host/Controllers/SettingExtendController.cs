using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.SettingManagement;

namespace Hugin.IdentityServer.Controllers
{
    [Area("setting")]
    [ControllerName("SettingExtend")]
    [Route("api/setting/setting-extend")]
    [Authorize]
    public class SettingExtendController : AbpController
    {
        private readonly ISettingManager _settingManager;

        public SettingExtendController(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        [HttpPost]
        [Route("current-tenant")]
        public async Task SetForCurrentTenantAsync([FromForm] string name, [FromForm] string value)
        {
            await _settingManager.SetForCurrentTenantAsync(name, value);
        }

        [HttpPost]
        [Route("current-user")]
        public async Task SetForCurrentUserAsync([FromForm] string name, [FromForm] string value)
        {
            await _settingManager.SetForCurrentUserAsync(name, value);
        }

        [HttpPost]
        [Route("tenant/{id}")]
        public async Task SetForTenantAsync(Guid id, [FromForm] string name, [FromForm] string value)
        {
            await _settingManager.SetForTenantAsync(id, name, value);
        }

        [HttpPost]
        [Route("user/{id}")]
        public async Task SetForUserAsync(Guid id, [FromForm] string name, [FromForm] string value)
        {
            await _settingManager.SetForUserAsync(id, name, value);
        }
    }
}
