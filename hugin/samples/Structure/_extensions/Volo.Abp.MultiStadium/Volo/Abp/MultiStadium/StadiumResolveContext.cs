using System;

namespace Volo.Abp.MultiStadium
{
    public class StadiumResolveContext : IStadiumResolveContext
    {
        public IServiceProvider ServiceProvider { get; }

        public string StadiumIdOrName { get; set; }

        public bool Handled { get; set; }

        public bool HasResolvedTenantOrHost()
        {
            return Handled || StadiumIdOrName != null;
        }

        public StadiumResolveContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}