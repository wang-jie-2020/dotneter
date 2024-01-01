namespace MinioStorage
{
    public class MinioStorageOptions
    {
        public MinioContainerConfigurations Containers { get; set; }

        public MinioStorageOptions()
        {
            Containers = new MinioContainerConfigurations();
        }
    }
}
