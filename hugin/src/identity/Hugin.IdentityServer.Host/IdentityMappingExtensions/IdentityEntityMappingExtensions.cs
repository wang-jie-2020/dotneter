using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Hugin.IdentityServer.IdentityMappingExtensions
{
    public static class IdentityEntityMappingExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ObjectExtensionManager.Instance.AddOrUpdateProperty<IdentityUser, string>("OpenId", options =>
                {
                    options.MapEfCore((entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(50);
                    });
                });

                ObjectExtensionManager.Instance.AddOrUpdateProperty<IdentityUser, string>("StadiumId", options =>
                {
                    options.MapEfCore((entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(500);
                    });
                });
            });
        }
    }
}
