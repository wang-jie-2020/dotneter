using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Hugin.BookStore.EntityFrameworkCore
{
    [ConnectionStringName(BookStoreConsts.DbProperties.ConnectionStringName)]
    public interface IBookStoreDbContext : IEfCoreDbContext
    {
    }
}