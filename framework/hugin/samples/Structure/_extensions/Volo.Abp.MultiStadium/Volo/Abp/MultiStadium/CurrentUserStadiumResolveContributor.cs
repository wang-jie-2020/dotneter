using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.MultiStadium
{
    public class CurrentUserStadiumResolveContributor : StadiumResolveContributorBase
    {
        public const string ContributorName = "CurrentUser";

        public override string Name => ContributorName;

        public override Task ResolveAsync(IStadiumResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            if (currentUser.IsAuthenticated)
            {
                if (currentUser.FindClaim("stadium") != null)
                {
                    context.Handled = true;
                    context.StadiumIdOrName = currentUser.FindClaim("stadium").Value;
                }
            }

            return Task.CompletedTask;
        }
    }
}
