using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiStadium
{
    public interface IStadiumResolveContext : IServiceProviderAccessor
    {
        [CanBeNull]
        string StadiumIdOrName { get; set; }

        bool Handled { get; set; }
    }
}