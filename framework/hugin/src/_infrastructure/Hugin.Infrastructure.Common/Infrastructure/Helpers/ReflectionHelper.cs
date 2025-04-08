using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hugin.Infrastructure.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// 指定程序集中查找某类型的实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetImplementsFromAssembly<T>(params Assembly[] assemblies)
        {
            return GetImplementsFromAssembly(typeof(T), assemblies);
        }

        /// <summary>
        /// 指定程序集中查找某类型的实现
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetImplementsFromAssembly(Type type, params Assembly[] assemblies)
        {
            var array = assemblies
                .Where(a => !a.IsDynamic)
                .SelectMany((Func<Assembly, IEnumerable<TypeInfo>>)(a => a.DefinedTypes))
                .ToArray();

            foreach (var element in array)
            {
                if (type.IsAssignableFrom(element) && !element.IsAbstract)
                    yield return element.AsType();
            }
        }
    }
}
