using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.EntityFrameworkCore.Extensions
{
    public static class ConfigureDbContextOptionsMySQLExtensions
    {
        public static void UseMySQL(
            this ConfigureDbContextOptions options,
            string connectionString,
            Action<MySqlDbContextOptionsBuilder> mySQLOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UseMySQL(connectionString, mySQLOptionsAction);
            });
        }

        public static void UseMySQL<TDbContext>(
            this ConfigureDbContextOptions options,
            string connectionString,
            Action<MySqlDbContextOptionsBuilder> mySQLOptionsAction = null)
            where TDbContext : DbContext
        {
            options.Configure<TDbContext>(context =>
            {
                context.UseMySQL(connectionString, mySQLOptionsAction);
            });
        }

        /*
         *  传入connectionString不够灵活
         */

        public static void UseMySQL(
            this ConfigureDbContextOptions options,
            Action<MySqlDbContextOptionsBuilder> mySQLOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UseMySQL(mySQLOptionsAction);
            });
        }

        public static void UseMySQL<TDbContext>(
            this ConfigureDbContextOptions options,
            Action<MySqlDbContextOptionsBuilder> mySQLOptionsAction = null)
            where TDbContext : DbContext
        {
            options.Configure<TDbContext>(context =>
            {
                context.UseMySQL(mySQLOptionsAction);
            });
        }
    }
}
