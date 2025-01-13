using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace MQ;

public class KafkaConverter : ISerializer<object>, IDeserializer<object>
{
    /// <summary>
    /// 序列化数据成字节
    /// </summary>
    /// <param name="data"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public byte[] Serialize(object data, SerializationContext context)
    {
        var json = JsonConvert.SerializeObject(data);
        return Encoding.UTF8.GetBytes(json);
    }

    public object Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) return null;

        var json = Encoding.UTF8.GetString(data.ToArray());
        try
        {
            return JsonConvert.DeserializeObject(json);
        }
        catch
        {
            return json;
        }
    }
}