using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Demo.EntityFrameworkCore.Extensions
{
    public class ConfigureDbContextOptions
    {
        internal Action<ConfigureDbContextOptionsBuilder> ConfigureAction { get; private set; }

        internal Dictionary<Type, object> ConfigureActions { get; private set; } = new Dictionary<Type, object>();

        public void Configure(Action<ConfigureDbContextOptionsBuilder> action)
        {
            ConfigureAction = action;
        }

        public void Configure<TDbContext>(Action<ConfigureDbContextOptionsBuilder<TDbContext>> action)
            where TDbContext : DbContext
        {
            ConfigureActions[typeof(TDbContext)] = action;
        }
    }
}
