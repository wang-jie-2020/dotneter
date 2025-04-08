using System;
using System.Threading.Tasks;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.EntityFrameworkCore
{
    public class DatabaseInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userContext = services.GetRequiredService<UserDbContext>();
            if (!await userContext.Users.AnyAsync())
            {
                userContext.Users.Add(new User()
                {
                    Id = 1,
                    Name = "1User"
                });
                await userContext.SaveChangesAsync();
            }

            var orderContext = services.GetRequiredService<OrderDbContext>();
            if (!await orderContext.Orders.AnyAsync())
            {
                orderContext.Orders.Add(new Order()
                {
                    Id = Guid.NewGuid(),
                    Name = "1Order"
                });
                await orderContext.SaveChangesAsync();
            }

            var productContext = services.GetRequiredService<ProductDbContext>();
            if (!await productContext.Products.AnyAsync())
            {
                productContext.Products.Add(new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "1Product"
                });
                await productContext.SaveChangesAsync();
            }
        }
    }
}