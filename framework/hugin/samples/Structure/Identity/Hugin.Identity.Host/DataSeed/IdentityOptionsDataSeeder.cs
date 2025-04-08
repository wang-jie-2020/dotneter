using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity.Settings;
using Volo.Abp.SettingManagement;

namespace Hugin.IdentityServer.DataSeed
{
    public class IdentityOptionsDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        public IdentityOptionsDataSeeder(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireDigit, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireLowercase, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireUppercase, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequiredUniqueChars, 0.ToString());
        }
    }
}
