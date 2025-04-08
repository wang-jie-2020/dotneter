using Hugin.BookStore.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Hugin.BookStoreWeb.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class BasePageModel : AbpPageModel
    {
        protected BasePageModel()
        {
            LocalizationResourceType = typeof(BookStoreResource);
            ObjectMapperContext = typeof(BookStoreWebModule);
        }
    }
}