using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiStadium;

namespace Hugin.BookStore.Stadiums
{
    public class StadiumStore : BaseDomainManager, IStadiumStore
    {
        private readonly IRepository<Stadium, Guid> _stadiumRepository;

        public StadiumStore(IRepository<Stadium, Guid> stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        public async Task<StadiumConfiguration> FindAsync(Guid id)
        {
            var stadium = await _stadiumRepository.GetAsync(t => t.Id == id);
            if (stadium == null)
                return null;

            return ObjectMapper.Map<Stadium, StadiumConfiguration>(stadium);
        }

        public async Task<StadiumConfiguration> FindAsync(string name)
        {
            var stadium = await _stadiumRepository.GetAsync(t => t.Name == name);
            if (stadium == null)
                return null;

            return ObjectMapper.Map<Stadium, StadiumConfiguration>(stadium);
        }
    }
}