using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Hugin.Platform.Stadiums
{
    public class DataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Stadium, Guid> _stadiumRepository;

        public DataSeederContributor(IRepository<Stadium, Guid> stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await Task.Delay(100);
        }
    }
}
