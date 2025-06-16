using System.Threading.Tasks;

namespace Yi.AspNetCore.DataSeed;

public interface IDataSeeder
{
    Task SeedAsync(DataSeedContext context);
}
