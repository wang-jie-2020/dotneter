using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Hugin.IdentityServer.RepositoryExtensions
{
    public static class IdentityUserRepositoryExtensions
    {
        public static async Task<IdentityUser> GetUserByOpenId(this IIdentityUserRepository identityUserRepository, string openId)
        {
            var context = identityUserRepository.GetDbContext();
            var dbSet = identityUserRepository.GetDbSet();

            return await dbSet.FirstOrDefaultAsync(p => EF.Property<string>(p, "OpenId") == openId);
        }
    }
}
