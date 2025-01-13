using System;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Demo.Aop;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Services
{
    public interface ICustomerService
    {
        void SyncVoid();

        int SyncInt();

        Task AsyncVoid();

        Task<int> AsyncInt();

        void Ex();
    }

    /*
     *  AOP实现的前提：
     *  （1）virtual 修饰
     *  （2）public 或者 protected 修饰
     *   (3) 满足以上2者，不存在其他情况
     *
     */
    [Intercept(typeof(ServiceInterceptor))]
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerManager _manager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomerService(ICustomerManager manager, IServiceScopeFactory serviceScopeFactory)
        {
            _manager = manager;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void SyncVoid()
        {
            _manager.SyncVoid();
        }

        public virtual int SyncInt()
        {
            return _manager.SyncInt();
        }

        public async Task AsyncVoid()
        {
            await _manager.AsyncVoid();
        }

        public virtual async Task<int> AsyncInt()
        {
            return await _manager.AsyncInt();
        }

        public void Ex()
        {
            Console.WriteLine("ExPro");
            ExPro();

            Console.WriteLine("ExVirtualPro");
            ExVirtualPro();
        }

        protected void ExPro()
        {

        }

        protected virtual void ExVirtualPro()
        {

        }
    }
}
