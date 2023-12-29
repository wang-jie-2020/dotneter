using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiStadium
{
    public class AbpStadiumResolveOptions
    {
        [NotNull]
        public List<IStadiumResolveContributor> StadiumResolvers { get; }

        public AbpStadiumResolveOptions()
        {
            StadiumResolvers = new List<IStadiumResolveContributor>
            {
                new CurrentUserStadiumResolveContributor()
            };
        }
    }
}
