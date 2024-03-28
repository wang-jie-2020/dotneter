using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESC.Sample.Permissions
{
    public class BookStorePermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BookStorePermissions));
        }

        public const string Default = "BookStore";

        public static class Book
        {
            public const string Default = $"{BookStorePermissions.Default}.Book";
            public const string Create = $"{Default}.Create";
            public const string Update = $"{Default}.Update";
        }

        public static class Author
        {
            public const string Default = $"{BookStorePermissions.Default}.Author";
            public const string Create = $"{Default}.Create";
            public const string Update = $"{Default}.Update";
        }
    }
}
