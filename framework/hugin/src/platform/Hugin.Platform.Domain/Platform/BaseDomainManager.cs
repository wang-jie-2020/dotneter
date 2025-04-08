using Hugin.Domain.Manager;

namespace Hugin.Platform
{
    public abstract class BaseDomainManager : HuginDomainManager
    {
        protected BaseDomainManager()
        {
            ObjectMapperContext = typeof(PlatformDomainModule);
        }
    }
}
