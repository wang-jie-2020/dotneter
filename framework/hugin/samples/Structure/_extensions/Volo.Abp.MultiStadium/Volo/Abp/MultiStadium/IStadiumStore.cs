using System;
using System.Threading.Tasks;

namespace Volo.Abp.MultiStadium
{
    public interface IStadiumStore
    {
        Task<StadiumConfiguration> FindAsync(Guid id);

        Task<StadiumConfiguration> FindAsync(string name);
    }
}