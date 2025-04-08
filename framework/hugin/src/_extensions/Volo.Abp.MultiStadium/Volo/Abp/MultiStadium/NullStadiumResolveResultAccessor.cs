using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiStadium
{
    public class NullStadiumResolveResultAccessor : IStadiumResolveResultAccessor, ISingletonDependency
    {
        public StadiumResolveResult Result
        {
            get => null;
            set { }
        }
    }
}