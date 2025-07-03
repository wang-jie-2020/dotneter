using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;

namespace Yi.AspNetCore.Mvc.ExceptionHandling;

public class ExceptionToErrorInfoConverter : ITransientDependency
{
    protected IStringLocalizer L { get; } 
    protected IServiceProvider ServiceProvider { get; }

    public ExceptionToErrorInfoConverter(
        IStringLocalizer stringLocalizer,
        IServiceProvider serviceProvider)
    {
        L = stringLocalizer;
        ServiceProvider = serviceProvider;
    }

    public AjaxResult Convert(Exception exception)
    {
        var errorInfo = CreateErrorInfoWithoutCode(exception);

        if (exception is IHasErrorCode hasErrorCodeException)
        {
            errorInfo.Code = hasErrorCodeException.Code;
        }

        return errorInfo;
    }

    protected virtual AjaxResult CreateErrorInfoWithoutCode(Exception exception)
    {
        exception = TryToGetActualException(exception);

        var errorInfo = AjaxResult.Error();

        if (exception is DbConcurrencyException)
        {
            errorInfo.Message = L["DbConcurrencyErrorMessage"];
        }

        if (exception is EntityNotFoundException entityNotFoundException)
        {
            errorInfo.Message = L["EntityNotFoundErrorMessage"];
            //errorInfo.Message = string.Format(L["EntityNotFoundErrorMessage"], entityNotFoundException.EntityType.Name, entityNotFoundException.Id);
        }
        
        if (exception is UnauthorizedException)
        {
            errorInfo.Message = L["UnauthorizedMessage"];
        }

        if (exception is NotImplementedException)
        {
            errorInfo.Message = L["NotImplementedMessage"];
        }
        
        if (exception is IUserFriendlyException)
        {
            errorInfo.Message = exception.Message;
            errorInfo.Details = (exception as IHasErrorDetails)?.Details;
        }

        TryToLocalizeExceptionMessage(exception, errorInfo);

        if (errorInfo.Message.IsNullOrEmpty())
        {
            errorInfo.Message = L["InternalServerErrorMessage"];
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
        
        var localizedString = L[exceptionWithErrorCode.Code];
        if (localizedString.ResourceNotFound)
        {
            return;
        }

        var localizedValue = localizedString.Value;
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