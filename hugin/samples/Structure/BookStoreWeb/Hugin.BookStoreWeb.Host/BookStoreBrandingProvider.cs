using Hugin.BookStore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Hugin.BookStoreWeb
{
    [Dependency(ReplaceServices = true)]
    public class BookStoreBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => BookStoreConsts.Name;
    }
}
