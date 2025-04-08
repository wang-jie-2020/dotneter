using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Sms
{
    public interface ISmsOptionsExtension
    {
        void AddServices(IServiceCollection services);
    }
}
