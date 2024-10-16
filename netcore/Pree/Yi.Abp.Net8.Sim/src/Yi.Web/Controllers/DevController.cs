using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;
using Yi.AspNetCore.System;
using Yi.AspNetCore.System.Events;

namespace Yi.Web.Controllers;

[ApiController]
[Route("dev-api")]
public class DevController : AbpController
{
    public DevController()
    {
    }

    [HttpGet("mvc")]
    public object MvcOptions()
    {
        // var uuid1 = GuidGenerator.Create();
        // Thread.Sleep(100);
        // var uuid2 = GuidGenerator.Create();
        // Thread.Sleep(100);
        // var uuid3 = GuidGenerator.Create();

        var abpLocalizationOptions = LazyServiceProvider.LazyGetRequiredService<IOptions<AbpLocalizationOptions>>().Value;

        var mvc = LazyServiceProvider.LazyGetRequiredService<IOptions<MvcOptions>>().Value;
        return new
        {
            mvc.Filters,
            mvc.Conventions,
            mvc.ModelBinderProviders,
            abpLocalizationOptions
        };
    }

    [HttpGet("success")]
    public AjaxResult MapSuccess()
    {
        return AjaxResult.Success();
    }

    [HttpGet("success2")]
    public AjaxResult<LoginEventArgs> MapSuccess2()
    {
        return AjaxResult<LoginEventArgs>.Success(new LoginEventArgs());
    }

    [HttpGet("error")]
    public AjaxResult MapError()
    {
        return AjaxResult.Error("123123123");
    }

    [HttpGet("exception")]
    public void MapException()
    {
        throw new NotImplementedException();
    }

    [HttpGet("authorizationException")]
    public void MapAuthorizationException()
    {
        throw new AbpAuthorizationException();
    }

    [HttpGet("files")]
    public object VirtualFile()
    {
        var virtualFileProvider = LazyServiceProvider.LazyGetRequiredService<IVirtualFileProvider>();

        var all = virtualFileProvider.GetDirectoryContents("").ToList();
        return new
        {
            embeds = virtualFileProvider.GetDirectoryContents("/Yi").ToList(),
            resources = virtualFileProvider.GetDirectoryContents("/Resources").ToList(),
        };
    }


    [HttpGet("lang")]
    public object Lang()
    {
        return L.GetAllStrings(true);
    }
    
    [HttpGet("lang2")]
    public object Lang2()
    {
        var localizer = StringLocalizerFactory.Create(typeof(AbpExceptionHandlingResource));
        return localizer.GetAllStrings(true);
    }

    // [HttpGet("lang")]
    // public object AppLang()
    // {
    //     // var localizer = StringLocalizerFactory.Create(typeof(AppResource));
    //     // return localizer.GetAllStrings();
    // }
    //
    // [HttpGet("default-local")]
    // public string DefaultLocal()
    // {
    //     return L["Permission:Query"];
    // }
    //
    // [HttpGet("app-local")]
    // public string AppLocal()
    // {
    //     // var localizer = StringLocalizerFactory.Create(typeof(AppResource));
    //     // return localizer["Permission:Query"];
    // }
}