using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Hugin.BookStore.Controllers
{
    [Route("api/healthCheck")]
    public class HealthCheckController : AbpController, IHealthCheckService
    {
        [HttpGet]
        public async Task<IActionResult> HeathCheck()
        {
            return await Task.FromResult(Ok());
        }

        [Authorize]
        [HttpGet]
        [Route("authorize")]
        public async Task<IActionResult> Authorize()
        {
            return await Task.FromResult(Ok());
        }
    }
}
