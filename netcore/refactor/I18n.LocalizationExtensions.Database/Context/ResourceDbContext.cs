using Microsoft.EntityFrameworkCore;

namespace I18n.LocalizationExtensions.Database.Context;

public class ResourceDbContext: DbContext
{
    public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Resource> Resources { get; set; }
}