using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.MultiStadium
{
    public interface IStadiumResolver
    {
        [NotNull]
        Task<StadiumResolveResult> ResolveStadiumIdOrNameAsync();
    }
}
