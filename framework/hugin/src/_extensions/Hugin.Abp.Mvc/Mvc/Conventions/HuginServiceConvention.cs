using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Hugin.Mvc.Conventions
{
    public class HuginServiceConvention : AbpServiceConvention
    {
        public HuginServiceConvention(IOptions<AbpAspNetCoreMvcOptions> options, IConventionalRouteBuilder conventionalRouteBuilder)
            : base(options, conventionalRouteBuilder)
        {

        }

        protected override void ConfigureApiExplorer(ActionModel action)
        {
            base.ConfigureApiExplorer(action);
            action.ApiExplorer.IsVisible = IsQueryableMethod(action.ActionMethod);
        }

        protected virtual bool IsQueryableMethod(MethodInfo method)
        {
            if (method.ReturnType.IsAssignableTo(typeof(IQueryable)))
            {
                return false;
            }

            return true;
        }

        protected override string SelectHttpMethod(ActionModel action, ConventionalControllerSetting configuration)
        {
            var httpMethod = base.SelectHttpMethod(action, configuration);
            if (httpMethod == "PUT" || httpMethod == "DELETE")
            {
                httpMethod = "POST";
            }

            return httpMethod;
        }
    }
}