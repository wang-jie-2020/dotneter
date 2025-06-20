using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiniExcelLibs;
using StackExchange.Profiling;
using Yi.AspNetCore;
using Yi.AspNetCore.Authorization;
using Yi.AspNetCore.Mvc;
using Yi.Framework.Core.Abstractions;
using Yi.System.Domains;
using Yi.System.Domains.Consts;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers;

[ApiController]
[Route("dev-api")]
public class DevController : BaseController
{
    public DevController()
    {
    }

    [HttpGet("mvc")]
    public object MvcOptions()
    {
        var mvcOptions = LazyServiceProvider.LazyGetRequiredService<IOptions<MvcOptions>>().Value;
        var miniProfiler = MiniProfiler.Current.RenderIncludes(HttpContext).ToString();
        
        return new
        {
            mvcOptions.Filters,
            mvcOptions.Conventions,
            mvcOptions.ModelBinderProviders,
            miniProfiler
        };
    }

    [HttpGet("success")]
    public AjaxResult MapSuccess()
    {
        return AjaxResult.Success(EventArgs.Empty);
    }

    [HttpGet("success2")]
    public AjaxResult<LoginEventArgs> MapSuccess2()
    {
        return AjaxResult<LoginEventArgs>.Success(new LoginEventArgs());
    }

    [HttpGet("exception")]
    public void MapException()
    {
        throw new Exception();
    }

    [HttpGet("authorizationException")]
    public void MapAuthorizationException()
    {
        throw new UnauthorizedException();
    }

    [HttpGet("businessException")]
    public void MapBusinessException()
    {
        throw Oops.Oh(AccountConst.VerificationCode_Invalid);
    }

    [HttpGet("lang")]
    public object Lang()
    {
        return L.GetAllStrings(true);
    }

    [HttpGet("lang2")]
    public object Lang2()
    {
        var localizer = StringLocalizerFactory.Create(typeof(DevController));
        return localizer.GetAllStrings(true);
    }

    [HttpGet("miniExcel")]
    public async Task<object> GetMiniExcel()
    {
        var list = new List<TenantDto>()
        {
            new()
            {
                Name = "123",
                CreationTime = DateTime.Now
            },
            new()
            {
                Name = "456",
                CreationTime = DateTime.Now
            }
        };

        var stream = new MemoryStream();
        await MiniExcel.SaveAsAsync(stream, list);
        stream.Seek(0, SeekOrigin.Begin);
        
        return new FileStreamResult(stream, "application/vnd.ms-excel")
        {
            FileDownloadName = $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}" + ".xlsx"
        };
    }
    
    [HttpGet("MagicExcel")]
    public async Task<object> GetMagicExcel()
    {
        var list = new List<TenantDto>()
        {
            new()
            {
                Name = "123",
                CreationTime = DateTime.Now
            },
            new()
            {
                Name = "456",
                CreationTime = DateTime.Now
            }
        };
        
        var buffer = await new ExcelExporter().ExportAsByteArray(list);
        return new FileContentResult(buffer, "application/vnd.ms-excel")
        {
            FileDownloadName = $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}" + ".xlsx"
        };
    }
}