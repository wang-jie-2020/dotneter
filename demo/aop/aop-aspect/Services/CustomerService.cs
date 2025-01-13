using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Demo.Aop;
using Microsoft.AspNetCore.Mvc;
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
     *  AOP的实现特点：
     *  （1）public 修饰
     *  （2）当调用者、被调用者属于同一个类时，不会重复出现aop，否则就会出现
     */
    [AspectCore.DynamicProxy.ServiceInterceptor(typeof(CustomerInterceptorAttribute))]  //相比castle，这样标注就足够执行aop了，不需要配置开启配置等等
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

        public int SyncInt()
        {
            return _manager.SyncInt();
        }

        public async Task AsyncVoid()
        {
            await _manager.AsyncVoid();
        }

        public async Task<int> AsyncInt()
        {
            return await _manager.AsyncInt();
        }

        [NonAspect]
        public void Ex()
        {
            Console.WriteLine("ExPro");
            ExPro();

            Console.WriteLine("ExPro2Manager");
            ExPro2Manager();
        }

        public void ExPro()
        {

        }

        public void ExPro2Manager()
        {
            _manager.SyncVoid();
            _manager.SyncInt();
        }
    }
}
