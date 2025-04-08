using Microsoft.Extensions.DependencyInjection;

namespace Hugin.Oss
{
    public interface IOssOptionsExtension
    {
        void AddServices(IServiceCollection services);
    }
}
