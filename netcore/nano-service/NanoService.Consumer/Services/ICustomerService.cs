using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace NanoService.Consumer.Services
{
    public interface ICustomerService : IHttpApi
    {
        [HttpGet("api/customer")]
        ITask<string> GetAsync();
    }
}
