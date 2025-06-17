using Volo.Abp.Collections;

namespace Yi.AspNetCore.Data.Seeding;

public class DataSeedOptions
{
    public DataSeedContributorList Contributors { get; }

    public DataSeedOptions()
    {
        Contributors = new DataSeedContributorList();
    }
}

public class DataSeedContributorList : TypeList<IDataSeedContributor>
{

}
