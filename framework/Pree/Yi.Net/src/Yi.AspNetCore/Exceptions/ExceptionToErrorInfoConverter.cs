using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Yi.AspNetCore.Core;

namespace Yi.AspNetCore.Exceptions;

public class ExceptionToErrorInfoConverter : ITransientDependency
{
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    //protected IStringLocalizer<AbpExceptionHandlingResource> L { get; } //todo i18n
    protected IServiceProvider ServiceProvider { get; }

    public ExceptionToErrorInfoConverter(
        IStringLocalizerFactory stringLocalizerFactory,
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        StringLocalizerFactory = stringLocalizerFactory;
    }

    public AjaxResult Convert(Exception exception)
    {
        var errorInfo = CreateErrorInfoWithoutCode(exception);

        if (exception is IHasErrorCode hasErrorCodeException)
        {
            errorInfo.Type = hasErrorCodeException.Code;
        }

        return errorInfo;
    }

    protected virtual AjaxResult CreateErrorInfoWithoutCode(Exception exception)
    {
        exception = TryToGetActualException(exception);

        var errorInfo = new AjaxResult();

        if (exception is IUserFriendlyException)
        {
            errorInfo.Message = exception.Message;
            errorInfo.Details = (exception as IHasErrorDetails)?.Details;
        }

        TryToLocalizeExceptionMessage(exception, errorInfo);

        if (errorInfo.Message.IsNullOrEmpty())
        {
            //todo i18n
            //errorInfo.Message = L["InternalServerErrorMessage"];
        }

        errorInfo.Data = exception.Data;

        return errorInfo;
    }

    protected virtual void TryToLocalizeExceptionMessage(Exception exception, AjaxResult errorInfo)
    {
        if (!(exception is IHasErrorCode exceptionWithErrorCode))
        {
            return;
        }

        //todo i18n
        //var stringLocalizer = StringLocalizerFactory.Create();
        //var localizedString = stringLocalizer[exceptionWithErrorCode.Code];
        // if (localizedString.ResourceNotFound)
        // {
        //     return;
        // }

        //var localizedValue = localizedString.Value;
        var localizedValue = "TODO";
        if (exception.Data != null && exception.Data.Count > 0)
        {
            foreach (var key in exception.Data.Keys)
            {
                localizedValue = localizedValue.Replace("{" + key + "}", exception.Data[key]?.ToString());
            }
        }

        errorInfo.Message = localizedValue;
    }

    protected virtual Exception TryToGetActualException(Exception exception)
    {
        if (exception is AggregateException aggException && aggException.InnerException != null)
        {
            return aggException.InnerException;
        }

        return exception;
    }
}