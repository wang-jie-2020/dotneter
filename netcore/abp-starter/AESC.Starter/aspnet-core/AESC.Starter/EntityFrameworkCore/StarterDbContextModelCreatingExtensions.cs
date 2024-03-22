namespace AESC.Starter.EntityFrameworkCore
{
    public static class StarterDbContextModelCreatingExtensions
    {
        public static void ConfigureStarter(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(StarterConsts.DbTablePrefix + "YourEntities", StarterConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}