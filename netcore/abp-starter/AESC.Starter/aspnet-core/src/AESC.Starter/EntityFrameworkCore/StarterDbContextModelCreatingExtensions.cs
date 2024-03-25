namespace AESC.Starter.EntityFrameworkCore
{
    public static class StarterDbContextModelCreatingExtensions
    {
        public static void ConfigureStarter(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
        }
    }
}