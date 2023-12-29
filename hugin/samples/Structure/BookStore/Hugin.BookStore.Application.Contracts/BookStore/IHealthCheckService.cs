using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Hugin.BookStore
{
    public interface IHealthCheckService : IApplicationService
    {
        Task<IActionResult> HeathCheck();

        Task<IActionResult> Authorize();
    }
}
