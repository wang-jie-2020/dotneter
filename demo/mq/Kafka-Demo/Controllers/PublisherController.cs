using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace MQ.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PublisherController : ControllerBase
{
    /// <summary>
    /// 异常事件
    /// </summary>
    public Action<object, Exception> ErrorHandler;

    /// <summary>
    /// 统计事件
    /// </summary>
    public Action<object, string> StatisticsHandler;

    [HttpGet]
    public async Task<object> Publish(string topic, int partition = -1)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = Global.SERVERS
        };

        var builder = new ProducerBuilder<string, object>(config);
        builder.SetValueSerializer(new KafkaConverter());
        Action<Delegate, object> tryCatchWrap = (@delegate, arg) =>
        {
            try
            {
                @delegate?.DynamicInvoke(arg);
            }
            catch
            {
                // ignored
            }
        };
        builder.SetErrorHandler((p, e) => tryCatchWrap(ErrorHandler, new Exception(e.Reason)));
        builder.SetStatisticsHandler((p, e) => tryCatchWrap(StatisticsHandler, e));

        using (var producer = builder.Build())
        {
            /*
                存在两种重载,行1实际上还是调用行2
                void Produce(string topic, Message<TKey, TValue> message, Action<DeliveryReport<TKey, TValue>> deliveryHandler = null);
                void Produce(TopicPartition topicPartition,Message<TKey, TValue> message, Action<DeliveryReport<TKey, TValue>> deliveryHandler = null);
             */

            var defaultKey = "kafka.publisher";
            var defaultValue = $"Published at {DateTime.Now.ToString("yyyyMMddHHmmss")}";

            var topicPartition = new TopicPartition(topic, new Partition(partition));
            await producer.ProduceAsync(topicPartition, new Message<string, object> { Key = defaultKey, Value = defaultValue });
        };

        return Ok();
    }
}