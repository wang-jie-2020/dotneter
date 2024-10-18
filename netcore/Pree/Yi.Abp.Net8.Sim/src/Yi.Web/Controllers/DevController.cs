using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;
using Yi.AspNetCore.System;
using Yi.AspNetCore.System.Events;
using Yi.Sys.Domains.Infra.Consts;
using Yi.Sys.Services.Infra.Impl;

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
        var mvcOptions = LazyServiceProvider.LazyGetRequiredService<IOptions<MvcOptions>>().Value;
        var abpLocalizationOptions = LazyServiceProvider.LazyGetRequiredService<IOptions<AbpLocalizationOptions>>().Value;
        
        return new
        {
            mvcOptions.Filters,
            mvcOptions.Conventions,
            mvcOptions.ModelBinderProviders,
            abpLocalizationOptions
        };
    }

    [HttpGet("success")]
    public AjaxResult MapSuccess()
    {
        return AjaxResult.Success(EventArgs.Empty);
    }

    [HttpGet("success2")]
    public AjaxResult<LoginEventArgs> MapSuccess2 ()
    {
        return AjaxResult<LoginEventArgs>.Success(new LoginEventArgs());
    }

    [HttpGet("error")]
    public AjaxResult MapError()
    {
        return AjaxResult.Error("123");
    }

    [HttpGet("exception")]
    public void MapException()
    {
        throw new Exception();
    }

    [HttpGet("authorizationException")]
    public void MapAuthorizationException()
    {
        throw new AbpAuthorizationException();
    }
    
    [HttpGet("businessException")]
    public void MapBusinessException()
    {
        throw Oops.Oh(UserConst.VerificationCode_Invalid);
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
}