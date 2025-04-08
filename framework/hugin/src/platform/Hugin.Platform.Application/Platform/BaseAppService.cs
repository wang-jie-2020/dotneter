using Hugin.Application.Services;
using Hugin.Platform.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Hugin.Platform
{
    [ApiExplorerSettings(GroupName = ApiGroups.Platform)]
    public abstract class BaseAppService : HuginApplicationService
    {
        protected BaseAppService()
        {
            LocalizationResource = typeof(PlatformResource);
            ObjectMapperContext = typeof(PlatformApplicationModule);
        }
    }
}
