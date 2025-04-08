using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Hugin.Platform.EntityFrameworkCore
{
    [ConnectionStringName(PlatformConsts.DbProperties.ConnectionStringName)]
    public interface IPlatformDbContext : IEfCoreDbContext
    {
    }
}