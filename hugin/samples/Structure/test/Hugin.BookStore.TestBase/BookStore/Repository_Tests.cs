using Volo.Abp.Modularity;

namespace Hugin.BookStore
{
    /* Write your custom repository tests like that, in this project, as abstract classes.
     * Then inherit these abstract classes from EF Core & MongoDB test projects.
     * In this way, both database providers are tests with the same set tests.
     */
    public abstract class Repository_Tests<TStartupModule> : TestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        //private readonly ISampleRepository _sampleRepository;

        protected Repository_Tests()
        {
            //_sampleRepository = GetRequiredService<ISampleRepository>();
        }
    }
}
