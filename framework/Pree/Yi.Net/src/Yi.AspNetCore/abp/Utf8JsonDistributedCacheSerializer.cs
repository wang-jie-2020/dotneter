using Newtonsoft.Json;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Caching;

public class Utf8JsonDistributedCacheSerializer : IDistributedCacheSerializer, ITransientDependency
{
    public Utf8JsonDistributedCacheSerializer()
    {
    }

    public byte[] Serialize<T>(T obj)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj!));
    }

    public T Deserialize<T>(byte[] bytes)
    {
        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
    }
}
