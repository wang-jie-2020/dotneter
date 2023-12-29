using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiStadium.ConfigurationStore
{
    [Dependency(TryRegister = true)]
    public class DefaultStadiumStore : IStadiumStore, ITransientDependency
    {
        private readonly AbpDefaultStadiumStoreOptions _options;

        public DefaultStadiumStore(IOptionsSnapshot<AbpDefaultStadiumStoreOptions> options)
        {
            _options = options.Value;
        }

        public Task<StadiumConfiguration> FindAsync(Guid id)
        {
            return Task.FromResult(Find(id));
        }

        public Task<StadiumConfiguration> FindAsync(string name)
        {
            return Task.FromResult(Find(name));
        }

        public StadiumConfiguration Find(Guid id)
        {
            return _options.Stadiums?.FirstOrDefault(t => t.Id == id);
        }

        public StadiumConfiguration Find(string name)
        {
            return _options.Stadiums?.FirstOrDefault(t => t.Name == name);
        }
    }
}