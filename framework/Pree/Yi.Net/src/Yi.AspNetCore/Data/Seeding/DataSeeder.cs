using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Yi.AspNetCore.Data.Seeding;

public class DataSeeder : IDataSeeder, ITransientDependency
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected DataSeedOptions Options { get; }

    public DataSeeder(
        IOptions<DataSeedOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync()
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            foreach (var contributorType in Options.Contributors)
            {
                var contributor = (IDataSeedContributor)scope.ServiceProvider.GetRequiredService(contributorType);
                await contributor.SeedAsync();
            }
        }
    }
}
