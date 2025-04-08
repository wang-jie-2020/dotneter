using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiStadium
{
    public class StadiumConfigurationProvider : IStadiumConfigurationProvider, ITransientDependency
    {
        protected virtual IStadiumResolver StadiumResolver { get; }
        protected virtual IStadiumStore StadiumStore { get; }
        protected virtual IStadiumResolveResultAccessor StadiumResolveResultAccessor { get; }

        public StadiumConfigurationProvider(
            IStadiumResolver stadiumResolver,
            IStadiumStore stadiumStore,
            IStadiumResolveResultAccessor stadiumResolveResultAccessor)
        {
            StadiumResolver = stadiumResolver;
            StadiumStore = stadiumStore;
            StadiumResolveResultAccessor = stadiumResolveResultAccessor;
        }

        public virtual async Task<StadiumConfiguration> GetAsync(bool saveResolveResult = false)
        {
            var resolveResult = await StadiumResolver.ResolveStadiumIdOrNameAsync();

            if (saveResolveResult)
            {
                StadiumResolveResultAccessor.Result = resolveResult;
            }

            StadiumConfiguration stadium = null;
            if (resolveResult.StadiumIdOrName != null)
            {
                stadium = await FindStadiumAsync(resolveResult.StadiumIdOrName);

                if (stadium == null)
                {
                    throw new BusinessException(
                        code: "Volo.AbpIo.MultiStadium:010001",
                        message: "Stadium not found!",
                        details: "There is no stadium with the stadium id or name: " + resolveResult.StadiumIdOrName
                    );
                }
            }

            return stadium;
        }

        protected virtual async Task<StadiumConfiguration> FindStadiumAsync(string stadiumIdOrName)
        {
            if (Guid.TryParse(stadiumIdOrName, out var parsedTenantId))
            {
                return await StadiumStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await StadiumStore.FindAsync(stadiumIdOrName);
            }
        }
    }
}
