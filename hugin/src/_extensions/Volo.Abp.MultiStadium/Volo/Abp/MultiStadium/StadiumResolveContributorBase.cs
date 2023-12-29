using System.Threading.Tasks;

namespace Volo.Abp.MultiStadium
{
    public abstract class StadiumResolveContributorBase : IStadiumResolveContributor
    {
        public abstract string Name { get; }

        public abstract Task ResolveAsync(IStadiumResolveContext context);
    }
}
