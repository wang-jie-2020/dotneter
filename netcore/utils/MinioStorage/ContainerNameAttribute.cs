using CheckCore;
using System;
using System.Reflection;

namespace MinioStorage
{
    public class ContainerNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ContainerNameAttribute(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
        }

        public static string GetContainerName<T>()
        {
            return GetContainerName(typeof(T));
        }

        public static string GetContainerName(Type type)
        {
            var nameAttribute = type.GetCustomAttribute<ContainerNameAttribute>();

            if (nameAttribute == null)
            {
                return type.FullName ?? throw new NullReferenceException();
            }

            return nameAttribute.Name;
        }
    }
}
