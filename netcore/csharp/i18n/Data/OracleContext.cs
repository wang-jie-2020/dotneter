using Microsoft.EntityFrameworkCore;

namespace i18n.Data;

public class OracleContext : DbContext
{
    public OracleContext(DbContextOptions<OracleContext> options) : base(options)
    {
    }

    public DbSet<DateTimeDemo> DateTimeDemos { get; set; }

    public DbSet<DateTimeOffsetDemo> DateTimeOffsetDemos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DateTimeDemo>().ToTable("DATETIMEDEMO");
        modelBuilder.Entity<DateTimeOffsetDemo>().ToTable("DATETIMEOFFSETDEMO");
        
        modelBuilder.Entity<DateTimeDemo>().Property(t => t.Id).HasColumnName("ID");
        modelBuilder.Entity<DateTimeDemo>().Property(t => t.Time1).HasColumnName("TIME1");
        modelBuilder.Entity<DateTimeDemo>().Property(t => t.Time2).HasColumnName("TIME2");
        modelBuilder.Entity<DateTimeDemo>().Property(t => t.Time3).HasColumnName("TIME3");
        modelBuilder.Entity<DateTimeDemo>().Property(t => t.Time4).HasColumnName("TIME4");
        
        modelBuilder.Entity<DateTimeOffsetDemo>().Property(t => t.Id).HasColumnName("ID");
        modelBuilder.Entity<DateTimeOffsetDemo>().Property(t => t.Time11).HasColumnName("TIME11");
        modelBuilder.Entity<DateTimeOffsetDemo>().Property(t => t.Time12).HasColumnName("TIME12");
        modelBuilder.Entity<DateTimeOffsetDemo>().Property(t => t.Time13).HasColumnName("TIME13");
        modelBuilder.Entity<DateTimeOffsetDemo>().Property(t => t.Time14).HasColumnName("TIME14");
    }
}