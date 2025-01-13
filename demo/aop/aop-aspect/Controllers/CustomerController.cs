using System.Threading.Tasks;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("sync")]
        public void SyncEmptyMethod()
        {
            _customerService.SyncVoid();
        }

        [HttpGet]
        [Route("sync-int")]
        public object SyncNonEmptyMethod()
        {
            return _customerService.SyncInt();
        }

        [HttpGet]
        [Route("async")]
        public async Task AsyncEmptyMethod()
        {
            await _customerService.AsyncVoid();
        }

        [HttpGet]
        [Route("async-int")]
        public async Task<object> AsyncNonEmptyMethod()
        {
            return await _customerService.AsyncInt();
        }

        [HttpGet]
        [Route("ex")]
        public void Ex()
        {
            _customerService.Ex();
        }
    }
}