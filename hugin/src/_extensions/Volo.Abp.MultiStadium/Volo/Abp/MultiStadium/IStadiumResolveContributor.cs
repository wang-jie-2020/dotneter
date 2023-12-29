using System.Threading.Tasks;

namespace Volo.Abp.MultiStadium
{
    public interface IStadiumResolveContributor
    {
        string Name { get; }

        Task ResolveAsync(IStadiumResolveContext context);
    }
}
