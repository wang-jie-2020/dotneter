using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Data
{
    public class DatabaseInitializer
    {
        public static void Seed(IServiceProvider services)
        {
            {
                var context = services.GetRequiredService<MSSQLDbContext>();
                context.GetService<IDatabaseCreator>().EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User()
                    {
                        Name = "zhangsan",
                        Name2 = "zhangsan",
                        Name3 = "zhangsan"
                    });

                    context.Users.Add(new User()
                    {
                        Name = "张三",
                        Name2 = "张三",
                        Name3 = "张三"
                    });
                }

                context.SaveChanges();
            }

            {
                var context = services.GetRequiredService<MSSQLDbContext>();
                var list = context.Users.ToList();
            }

            {
                var context = services.GetRequiredService<MySQLDbContext>();
                context.GetService<IDatabaseCreator>().EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User()
                    {
                        Name = "zhangsan",
                        Name2 = "zhangsan",
                        Name3 = "zhangsan"
                    });

                    context.Users.Add(new User()
                    {
                        Name = "张三",
                        Name2 = "张三",
                        Name3 = "张三"
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
