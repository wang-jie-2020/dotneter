namespace Yi.AspNetCore.Data.Seeding;

public interface IDataSeeder
{
    Task SeedAsync();
}