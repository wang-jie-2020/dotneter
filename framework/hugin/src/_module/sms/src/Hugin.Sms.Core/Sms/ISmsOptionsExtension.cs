using Microsoft.Extensions.DependencyInjection;

namespace Hugin.Sms
{
    public interface ISmsOptionsExtension
    {
        void AddServices(IServiceCollection services);
    }
}
