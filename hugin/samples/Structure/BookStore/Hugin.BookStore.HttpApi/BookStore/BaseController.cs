using Hugin.BookStore.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Hugin.BookStore
{
    [ApiExplorerSettings(GroupName = ApiGroups.BookStore)]
    public abstract class BaseController : AbpController
    {
        protected BaseController()
        {
            LocalizationResource = typeof(BookStoreResource);
        }
    }
}
