using Volo.Abp.DependencyInjection;
using Yi.System;

namespace Yi.Web;

public class YiDbContext: YiRbacDbContext
{
    public YiDbContext(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
    }
}