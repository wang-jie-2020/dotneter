using Volo.Abp.Collections;

namespace Yi.AspNetCore.Data.Seeding;

public class DataSeedOptions
{
    public TypeList<IDataSeedContributor> Contributors { get; }

    public DataSeedOptions()
    {
        Contributors = new TypeList<IDataSeedContributor>();
    }
}
