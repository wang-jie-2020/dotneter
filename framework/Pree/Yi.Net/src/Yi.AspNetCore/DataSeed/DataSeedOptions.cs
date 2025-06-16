namespace Yi.AspNetCore.DataSeed;

public class DataSeedOptions
{
    public DataSeedContributorList Contributors { get; }

    public DataSeedOptions()
    {
        Contributors = new DataSeedContributorList();
    }
}
