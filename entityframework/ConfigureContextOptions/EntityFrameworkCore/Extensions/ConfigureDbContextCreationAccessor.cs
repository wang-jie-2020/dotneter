using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.EntityFrameworkCore.Extensions
{
    public class ConfigureDbContextCreationAccessor
    {
        public string ConnectionString { get; set; }

        private static AsyncLocal<ConfigureDbContextCreationAccessor> _current = new AsyncLocal<ConfigureDbContextCreationAccessor>();

        public static ConfigureDbContextCreationAccessor Current => _current.Value;

        public ConfigureDbContextCreationAccessor(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
