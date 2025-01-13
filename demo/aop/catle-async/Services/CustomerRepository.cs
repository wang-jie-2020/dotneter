using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Demo.Aop;

namespace Demo.Services
{
    public interface ICustomerRepository
    {
        void SyncVoid();

        int SyncInt();

        Task AsyncVoid();

        Task<int> AsyncInt();

    }

    public class CustomerRepository : ICustomerRepository
    {
        public void SyncVoid()
        {

        }

        public virtual int SyncInt()
        {
            return 1;
        }

        public async Task AsyncVoid()
        {
            await Task.CompletedTask;
        }

        public virtual async Task<int> AsyncInt()
        {
            return await Task.FromResult(1);
        }
    }
}
