using System.Threading.Tasks;
using Hugin.Domain.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiStadium;

namespace Hugin.Terminal
{
    public class DataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IGuidGenerator _guidGenerator;

        public DataSeederContributor(IDataFilter dataFilter,
            IGuidGenerator guidGenerator)
        {
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_dataFilter.Disable<IMultiUser>())
            {
                using (_dataFilter.Disable<IMultiStadium>())
                {
                    await Task.Delay(100);
                }
            }
        }
    }
}
