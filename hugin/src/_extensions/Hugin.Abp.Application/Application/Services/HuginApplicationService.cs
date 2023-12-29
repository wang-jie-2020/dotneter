using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiStadium;

namespace Hugin.Application.Services
{
    public abstract class HuginApplicationService : ApplicationService
    {
        private IHttpContextAccessor _httpContextAccessor;
        protected IHttpContextAccessor HttpContextAccessor => this.LazyGetRequiredService<IHttpContextAccessor>(ref this._httpContextAccessor);

        protected HttpContext HttpContext => HttpContextAccessor.HttpContext;

        private IMapperAccessor _mapperAccessor;
        protected IMapperAccessor MapperAccessor => this.LazyGetRequiredService<IMapperAccessor>(ref this._mapperAccessor);

        private ILocalEventBus _localEventBus;
        protected ILocalEventBus LocalEventBus => this.LazyGetRequiredService<ILocalEventBus>(ref this._localEventBus);

        private ICurrentStadium _currentStadium;
        protected ICurrentStadium CurrentStadium => LazyGetRequiredService(ref _currentStadium);

        protected HuginApplicationService()
        {

        }
    }
}
