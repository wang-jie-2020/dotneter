using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Demo.Aop;

namespace Demo.Services
{
    public interface ICustomerManager
    {
        void SyncVoid();

        int SyncInt();

        Task AsyncVoid();

        Task<int> AsyncInt();

    }

    public class CustomerManager : ICustomerManager
    {
        public void SyncVoid()
        {

        }

        public int SyncInt()
        {
            return 1;
        }

        public async Task AsyncVoid()
        {
            await Task.CompletedTask;
        }

        public async Task<int> AsyncInt()
        {
            return await Task.FromResult(1);
        }
    }
}
