using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.SettingManagement;

namespace Hugin.IdentityServer.Controllers
{
    /*
     *  1.SettingDefinitionProvider 定义，虽然在示例中是在Domain定义的，但在现在的结构下，通过模块的ApplicationContracts和DomainShared或许更好
     *  2.预定义的模块中未提供相关的UI和API，这里进行一些补充，但只针对租户或者用户，全局还是不合适的
     *  3.全局配置的修改的IdentityOptions
     *  4.不需要远程访问吧？对于远程客户端，RemoteSettingProvider足矣
     */
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
