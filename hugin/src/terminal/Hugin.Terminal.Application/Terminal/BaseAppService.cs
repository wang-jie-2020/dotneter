using Hugin.Application.Services;
using Hugin.Platform.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Hugin.Terminal
{
    [ApiExplorerSettings(GroupName = ApiGroups.Terminal)]
    public abstract class BaseAppService : HuginApplicationService
    {
        protected BaseAppService()
        {
            LocalizationResource = typeof(PlatformResource);
            ObjectMapperContext = typeof(TerminalApplicationModule);
        }
    }
}
