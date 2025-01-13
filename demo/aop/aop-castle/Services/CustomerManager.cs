using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
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
        private readonly ICustomerRepository _repository;

        public CustomerManager(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void SyncVoid()
        {
            _repository.SyncVoid();
        }

        public virtual int SyncInt()
        {
            return _repository.SyncInt();
        }

        public async Task AsyncVoid()
        {
            await _repository.AsyncVoid();
        }

        public virtual async Task<int> AsyncInt()
        {
            return await _repository.AsyncInt();
        }
    }
}
