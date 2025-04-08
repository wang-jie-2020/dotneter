using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.Http;

namespace Hugin.Mvc.ResultHandling
{
    public class HuginResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (ShouldHandleResult(context))
            {
                await HandleAndWrapResult(context);
            }

            await next();
        }

        protected virtual bool ShouldHandleResult(ResultExecutingContext context)
        {
            if (context.ActionDescriptor.IsControllerAction() &&
                context.ActionDescriptor.HasObjectResult())
            {
                return true;
            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }

        protected virtual async Task HandleAndWrapResult(ResultExecutingContext context)
        {
            var result = (context.Result as ObjectResult)?.Value;
            if (result != null)
            {
                var response = new HuginRemoteServiceSuccessResponse(result);
                context.Result = new ObjectResult(response);

            }
            await Task.CompletedTask;
        }
    }
}
