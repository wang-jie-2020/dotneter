using Volo.Abp.DependencyInjection;
using Yi.Sys;

namespace Yi.Web;

public class YiDbContext: YiDataScopedDbContext
{
    public YiDbContext(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
    }
}