using Hugin.Domain.Manager;

namespace Hugin.Terminal
{
    public abstract class BaseDomainManager : HuginDomainManager
    {
        protected BaseDomainManager()
        {
            ObjectMapperContext = typeof(TerminalDomainModule);
        }
    }
}
