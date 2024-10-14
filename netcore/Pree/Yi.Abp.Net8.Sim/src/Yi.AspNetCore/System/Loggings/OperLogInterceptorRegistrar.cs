using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Yi.AspNetCore.System.Loggings;

public static class OperLogInterceptorRegistrar
{
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<OperLogInterceptor>();
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        return !DynamicProxyIgnoreTypes.Contains(type) &&
               (type.IsDefined(typeof(OperLogAttribute), true) || AnyMethodHasAttribute((type)));
    }

    private static bool AnyMethodHasAttribute(Type implementationType)
    {
        return implementationType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasAttribute);
    }

    private static bool HasAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(OperLogAttribute), true);
    }
}