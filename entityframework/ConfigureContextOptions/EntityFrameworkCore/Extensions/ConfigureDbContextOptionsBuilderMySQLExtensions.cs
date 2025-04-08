using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.EntityFrameworkCore.Extensions
{
    public static class ConfigureDbContextOptionsBuilderMySQLExtensions
    {
        public static DbContextOptionsBuilder UseMySQL(
            this ConfigureDbContextOptionsBuilder builder,
            string connectionString,
            Action<MySqlDbContextOptionsBuilder> mySQLOptionsAction = null)
        {
            return builder.DbContextOptionsBuilder.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString), optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    mySQLOptionsAction?.Invoke(optionsBuilder);
                });
        }

        /*
         *  传入connectionString不够灵活
         */

        public static DbContextOptionsBuilder UseMySQL(
            this ConfigureDbContextOptionsBuilder builder,
            Action<MySqlDbContextOptionsBuilder> mySQLOptionsAction = null)
        {
            return builder.DbContextOptionsBuilder.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString), optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    mySQLOptionsAction?.Invoke(optionsBuilder);
                });
        }
    }
}
