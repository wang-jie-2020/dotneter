using Microsoft.Extensions.DependencyInjection;

namespace Demo.Core
{
    public interface ICoreOptionsExtension
    {
        void AddServices(IServiceCollection services);
    }
}
