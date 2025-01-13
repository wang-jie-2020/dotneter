using System;
using System.Collections.Generic;
using Demo.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Data
{
    public class DatabaseInitializer
    {
        public static void Seed(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var connection = services.GetService<SqliteConnection>();

            connection.Open();
            context.GetService<IDatabaseCreator>().EnsureCreated();

            context.Users.Add(new User()
            {
                Id = 1,
                Name = "no.1"
            });

            context.SaveChanges();
        }
    }
}
