using Volo.Abp.DependencyInjection;
using Yi.Infra.Rbac;

namespace Yi.Web;

public class YiDbContext: YiRbacDbContext
{
    public YiDbContext(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
    }
}