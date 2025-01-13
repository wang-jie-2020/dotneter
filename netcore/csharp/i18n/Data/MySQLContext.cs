using Microsoft.EntityFrameworkCore;

namespace i18n.Data;

public class MySqlContext : DbContext
{
    public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
    {
        
    }
    
    public DbSet<DateTimeDemo> DateTimeDemos { get; set; }
    
    public DbSet<DateTimeOffsetDemo> DateTimeOffsetDemos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DateTimeDemo>().ToTable("DateTimeDemo");
        modelBuilder.Entity<DateTimeOffsetDemo>().ToTable("DateTimeOffsetDemo");
    }
}