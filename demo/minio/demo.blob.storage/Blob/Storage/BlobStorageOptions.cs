using Microsoft.Extensions.DependencyInjection;

namespace Demo.Blob.Storage
{
    public class BlobStorageOptions
    {
        public BlobContainerConfigurations Containers { get; }

        public IServiceCollection ServicesCollection { get; set; }

        public BlobStorageOptions()
        {
            Containers = new BlobContainerConfigurations();
        }

        public BlobStorageOptions(IServiceCollection services) : this()
        {
            ServicesCollection = services;
        }
    }
}