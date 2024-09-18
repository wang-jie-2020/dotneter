using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Yi.Admin.Controllers;

[ApiController]
[Route("api/app/file")]
public class FileController : AbpController
{
    [HttpPost]
    public async Task<List<IActionResult>> Upload([FromForm] IFormFileCollection file)
    {
        throw new NotImplementedException();
    }
}