namespace Yi.AspNetCore.Data.Seeding;

public interface IDataSeedContributor
{
    Task SeedAsync();
}
