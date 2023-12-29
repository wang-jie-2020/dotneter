using System;
using System.Threading.Tasks;
using Hugin.Domain.Entities;
using Hugin.Domain.Repository;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Hugin.BookStore.Stadiums
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
            await _stadiumRepository.InsertOnlyNotExistAsync(new Stadium(null, "体育馆")
                .TrySetId(new Guid("0a301ffa-a83d-4c5e-9521-64eabaa5c329")), autoSave: true);

            await _stadiumRepository.InsertOnlyNotExistAsync(new Stadium(null, "游泳馆")
                .TrySetId(new Guid("a99863a7-3f2c-491b-b4d8-b549cd2fd0f5")), autoSave: true);
        }
    }
}
