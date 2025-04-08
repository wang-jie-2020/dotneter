using System.Threading.Tasks;

namespace Volo.Abp.MultiStadium
{
    public interface IStadiumConfigurationProvider
    {
        Task<StadiumConfiguration> GetAsync(bool saveResolveResult = false);
    }
}
