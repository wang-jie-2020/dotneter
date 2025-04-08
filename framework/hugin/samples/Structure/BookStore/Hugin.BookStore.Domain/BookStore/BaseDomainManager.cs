using Hugin.Domain.Manager;

namespace Hugin.BookStore
{
    public abstract class BaseDomainManager : HuginDomainManager
    {
        protected BaseDomainManager()
        {
            ObjectMapperContext = typeof(BookStoreDomainModule);
        }
    }
}
