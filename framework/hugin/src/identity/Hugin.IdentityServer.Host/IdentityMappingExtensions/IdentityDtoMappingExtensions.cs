using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Hugin.IdentityServer.IdentityMappingExtensions
{
    public static class IdentityDtoMappingExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
                    new[]
                    {
                        typeof(IdentityUserDto),
                        typeof(IdentityUserCreateDto),
                        typeof(IdentityUserUpdateDto)
                    },
                    "OpenId", options =>
                    {
                        options.MapEfCore();
                    });

                ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
                    new[]
                    {
                            typeof(IdentityUserDto),
                            typeof(IdentityUserCreateDto),
                            typeof(IdentityUserUpdateDto)
                    },
                    "StadiumId", options =>
                    {
                        options.MapEfCore();
                    });
            });
        }
    }
}
