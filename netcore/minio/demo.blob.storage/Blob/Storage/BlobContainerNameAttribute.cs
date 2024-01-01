﻿using Demo.Blob.Storage.Utils;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace Demo.Blob.Storage
{
    public class BlobContainerNameAttribute : Attribute
    {
        [NotNull]
        public string Name { get; }

        public BlobContainerNameAttribute([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
        }

        public virtual string GetName(Type type)
        {
            return Name;
        }

        public static string GetContainerName<T>()
        {
            return GetContainerName(typeof(T));
        }

        public static string GetContainerName(Type type)
        {
            var nameAttribute = type.GetCustomAttribute<BlobContainerNameAttribute>();

            if (nameAttribute == null)
            {
                return type.FullName;
            }

            return nameAttribute.GetName(type);
        }
    }
}
