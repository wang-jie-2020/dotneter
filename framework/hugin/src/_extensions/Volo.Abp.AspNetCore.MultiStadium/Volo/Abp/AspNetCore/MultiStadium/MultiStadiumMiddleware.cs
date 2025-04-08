using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiStadium;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public class MultiStadiumMiddleware : IMiddleware, ITransientDependency
    {
        private readonly IStadiumConfigurationProvider _stadiumConfigurationProvider;
        private readonly ICurrentStadium _currentStadium;
        private readonly ICurrentUser _currentUser;

        public MultiStadiumMiddleware(
            IStadiumConfigurationProvider stadiumConfigurationProvider,
            ICurrentStadium currentStadium,
            ICurrentUser currentUser)
        {
            _stadiumConfigurationProvider = stadiumConfigurationProvider;
            _currentStadium = currentStadium;
            _currentUser = currentUser;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stadium = await _stadiumConfigurationProvider.GetAsync(saveResolveResult: true);

            using (_currentStadium.Change(stadium?.Id, stadium?.Name))
            {
                if (!IsGrantedStadium(stadium))
                {
                    throw new BusinessException(
                        code: "Volo.AbpIo.MultiStadium:020001",
                        message: "Stadium not granted!",
                        details: "Current User is not granted with the stadium id: " + stadium?.Id
                    );
                }

                await next(context);
            }
        }

        /*
         * 检查User是否授权操作
         * 1.若是admin，则不必考虑
         * 2.若是其他角色又未授权，则直接抛出错误
         */
        protected virtual bool IsGrantedStadium(StadiumConfiguration stadium)
        {
            if (stadium == null)
                return true;

            if (!_currentUser.IsAuthenticated)
                return true;

            if (_currentUser.UserName == "admin" || _currentUser.IsInRole("admin"))
                return true;

            var grantedStadiums = _currentUser.FindClaim("stadiums")?.Value ?? string.Empty;
            if (grantedStadiums.Contains(stadium.Id.ToString()))
            {
                return true;
            }

            return false;
        }
    }
}
