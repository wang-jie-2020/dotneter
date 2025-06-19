using Microsoft.EntityFrameworkCore;

namespace i18n.Data;

public class MsSqlContext : DbContext
{
    public MsSqlContext(DbContextOptions<MsSqlContext> options) : base(options)
    {
    }

    public DbSet<DateTimeDemo> DateTimeDemos { get; set; }
    
    public DbSet<DateTimeOffsetDemo> DateTimeOffsetDemos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DateTimeDemo>().ToTable("DateTimeDemo");
        modelBuilder.Entity<DateTimeOffsetDemo>().ToTable("DateTimeOffsetDemo");
        
        modelBuilder.Entity<DateTimeDemo>().Property(p => p.Time1).HasColumnType("datetime");
        modelBuilder.Entity<DateTimeDemo>().Property(p => p.Time2).HasColumnType("datetime");
        modelBuilder.Entity<DateTimeDemo>().Property(p => p.Time3).HasColumnType("datetime");
        modelBuilder.Entity<DateTimeDemo>().Property(p => p.Time4).HasColumnType("datetime");
    }
}