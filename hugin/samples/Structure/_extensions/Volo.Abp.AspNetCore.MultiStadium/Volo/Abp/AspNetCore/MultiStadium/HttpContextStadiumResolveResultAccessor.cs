using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    [Dependency(ReplaceServices = true)]
    public class HttpContextStadiumResolveResultAccessor : IStadiumResolveResultAccessor, ITransientDependency
    {
        public const string HttpContextItemName = "__AbpTenantResolveResult";

        public StadiumResolveResult Result
        {
            get => _httpContextAccessor.HttpContext?.Items[HttpContextItemName] as StadiumResolveResult;
            set
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return;
                }

                _httpContextAccessor.HttpContext.Items[HttpContextItemName] = value;
            }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextStadiumResolveResultAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}