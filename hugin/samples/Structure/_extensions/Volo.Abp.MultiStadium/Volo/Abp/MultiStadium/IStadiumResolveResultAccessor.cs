using JetBrains.Annotations;

namespace Volo.Abp.MultiStadium
{
    public interface IStadiumResolveResultAccessor
    {
        [CanBeNull]
        StadiumResolveResult Result { get; set; }
    }
}
