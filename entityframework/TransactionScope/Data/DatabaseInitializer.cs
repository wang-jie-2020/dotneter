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
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.GetService<IDatabaseCreator>().EnsureCreated();


            var list1 = new List<Author>();
            for (int i = 0; i < 50; i++)
            {
                list1.Add(new Author()
                {
                    Name = i.ToString()
                });
            }

            var list2 = new List<Book>();
            for (int i = 0; i < 50; i++)
            {
                list2.Add(new Book()
                {
                    Name = i.ToString()
                });
            }

            if (!context.Authors.Any())
            {
                context.Authors.AddRange(list1);
                context.SaveChanges();
            }

            if (!context.Books.Any())
            {
                context.Books.AddRange(list2);
                context.SaveChanges();
            }
        }
    }
}
