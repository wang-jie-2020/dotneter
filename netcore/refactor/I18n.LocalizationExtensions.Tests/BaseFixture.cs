using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;

namespace I18n.LocalizationExtensions.Tests;

// public class BaseFixture: IDisposable
// {
//     public IServiceProvider ServiceProvider { get; }
//     
//     public BaseFixture()
//     {
//         var services = new ServiceCollection();
//         services.AddJsonLocalization();
//         ServiceProvider = services.BuildServiceProvider();
//     }
//     
//     public void Dispose()
//     {
//
//     }
// }
//
// [CollectionDefinition("base-collection")]
// public class BaseCollection : ICollectionFixture<BaseFixture>
// {
//     
// }
//
// [Collection("base-collection")]
// public class BaseTest
// {
//     private readonly BaseFixture _baseFixture;
//
//     public BaseTest(BaseFixture baseFixture)
//     {
//         _baseFixture = baseFixture;
//     }
// }