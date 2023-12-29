using System;
using System.Collections.Generic;
using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace Hugin.Mvc.DependencyInjection
{
    public class HuginDefaultConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            var types = base.GetExposedServiceTypes(type);

            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                var interfaceName = interfaceType.Name;
                if (interfaceName.EndsWith("DaoService", StringComparison.OrdinalIgnoreCase))
                {
                    types.Add(interfaceType);
                }
            }

            return types;
        }
    }
}
