using Hugin.Platform.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Hugin.Platform
{
    [ApiExplorerSettings(GroupName = ApiGroups.Platform)]
    public abstract class BaseController : AbpController
    {
        protected BaseController()
        {
            LocalizationResource = typeof(PlatformResource);
        }
    }
}
