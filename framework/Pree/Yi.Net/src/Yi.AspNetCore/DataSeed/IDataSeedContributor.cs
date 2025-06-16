namespace Yi.AspNetCore.DataSeed;

public interface IDataSeedContributor
{
    Task SeedAsync(DataSeedContext context);
}
