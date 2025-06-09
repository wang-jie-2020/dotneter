using Volo.Abp.DependencyInjection;
using Yi.System;

namespace Yi.Web;

public class YiDbContext: YiDataScopedDbContext
{
    public YiDbContext(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
    }
}