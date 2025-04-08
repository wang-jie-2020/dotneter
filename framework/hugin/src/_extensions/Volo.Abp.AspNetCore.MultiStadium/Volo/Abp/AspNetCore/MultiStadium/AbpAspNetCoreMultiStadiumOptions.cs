using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public class AbpAspNetCoreMultiStadiumOptions
    {
        public string StadiumKey { get; set; }

        public AbpAspNetCoreMultiStadiumOptions()
        {
            StadiumKey = StadiumResolverConsts.DefaultStadiumKey;
        }
    }
}