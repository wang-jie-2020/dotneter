using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MQ.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ConsumerController : ControllerBase
{
    /// <summary>
    /// 异常事件
    /// </summary>
    public Action<object, Exception> ErrorHandler;

    /// <summary>
    /// 统计事件
    /// </summary>
    public Action<object, string> StatisticsHandler;

    public ConsumerController()
    {
        ErrorHandler += (o, exception) =>
        {
            Console.WriteLine(exception.Message);
        };

        StatisticsHandler += (o, s) =>
        {
            Console.WriteLine(s);
        };
    }

    [HttpGet]
    public async Task Consume(string topic, int partition = -1)
    {
        //var ctx = CancellationTokenSource.CreateLinkedTokenSource(new CancellationTokenSource(60000).Token, HttpContext.RequestAborted).Token;
        var ctx = new CancellationTokenSource(60000).Token;

        new Task(() =>
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = Global.SERVERS,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                GroupId = "A"
            };

            var builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter());
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

            using (var consumer = builder.Build())
            {

                if (partition == -1)
                {
                    consumer.Subscribe(topic);
                }
                else
                {
                    consumer.Assign(new TopicPartition(topic, partition));
                }

                while (true)
                {
                    var result = consumer.Consume();
                    consumer.Commit(result);

                    Console.WriteLine($"message received:{result.Message.Value}");
                }
            }
        }, ctx).Start();

        await Task.CompletedTask;
    }
}