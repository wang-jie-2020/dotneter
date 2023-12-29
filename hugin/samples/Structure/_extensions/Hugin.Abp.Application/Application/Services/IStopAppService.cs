using System.Threading.Tasks;

namespace Hugin.Application.Services
{
    public interface IStopAppService<in TKey>
    {
        Task Stop(TKey id);

        Task CancelStop(TKey id);
    }
}