using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiStadium
{
    public class StadiumResolver : IStadiumResolver, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AbpStadiumResolveOptions _options;

        public StadiumResolver(IOptions<AbpStadiumResolveOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public virtual async Task<StadiumResolveResult> ResolveStadiumIdOrNameAsync()
        {
            var result = new StadiumResolveResult();

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = new StadiumResolveContext(serviceScope.ServiceProvider);

                foreach (var stadiumResolver in _options.StadiumResolvers)
                {
                    await stadiumResolver.ResolveAsync(context);

                    result.AppliedResolvers.Add(stadiumResolver.Name);

                    if (context.HasResolvedTenantOrHost())
                    {
                        result.StadiumIdOrName = context.StadiumIdOrName;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
