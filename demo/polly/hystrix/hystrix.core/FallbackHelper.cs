using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace hystrix.core
{
    public static class FallbackHelper
    {
        public static bool IsFallbackType(TypeInfo implementationType)
        {
            if (HasFallbackAttribute(implementationType) || AnyMethodHasFallbackAttribute(implementationType))
            {
                return true;
            }

            return false;
        }

        public static bool IsFallbackMethod([NotNull] MethodInfo methodInfo, [CanBeNull] out FallbackAttribute fallbackAttribute)
        {
            //Method declaration
            var attrs = methodInfo.GetCustomAttributes(true).OfType<FallbackAttribute>().ToArray();
            if (attrs.Any())
            {
                fallbackAttribute = attrs.First();
                return true;
            }

            if (methodInfo.DeclaringType != null)
            {
                //Class declaration
                attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<FallbackAttribute>().ToArray();
                if (attrs.Any())
                {
                    fallbackAttribute = attrs.First();
                    return true;
                }
            }

            fallbackAttribute = null;
            return false;
        }

        public static FallbackAttribute GetFallbackAttributeOrNull(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<FallbackAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<FallbackAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            return null;
        }

        private static bool AnyMethodHasFallbackAttribute(TypeInfo implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(HasFallbackAttribute);
        }

        private static bool HasFallbackAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(FallbackAttribute), true);
        }
    }
}
