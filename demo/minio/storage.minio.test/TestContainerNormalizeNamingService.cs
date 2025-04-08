//using MinioStorage;

//namespace Demo
//{
//    public class TestContainerNormalizeNamingService : ITestContainerNormalizeNamingService
//    {
//        public (string, string) NormalizeNaming(MinioContainerConfiguration configuration, string containerName, string objectName)
//        {
//            return (containerName, objectName);
//        }

//        public string NormalizeContainerNaming(MinioContainerConfiguration configuration, string name)
//        {
//            return NormalizeNaming(configuration, name, null).Item1;
//        }

//        public string NormalizeObjectNaming(MinioContainerConfiguration configuration, string name)
//        {
//            return NormalizeNaming(configuration, null, name).Item2;
//        }
//    }

//    public interface ITestContainerNormalizeNamingService : INormalizeNamingService { }


//}
