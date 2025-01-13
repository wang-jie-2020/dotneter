using Microsoft.AspNetCore.Mvc;

namespace MQ.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class Demo3Controller : ControllerBase
{
    // static async Task Main()
    // {
    //     var config = new AdminClientConfig
    //     {
    //         BootstrapServers = "192.168.3.158:19092"
    //     };
    //
    //     using (var adminClient = new AdminClientBuilder(config).Build())
    //     {
    //         try
    //         {
    //             await adminClient.CreateTopicsAsync(new TopicSpecification[] {
    //                 new TopicSpecification { Name = "hello-topic2", ReplicationFactor = 3, NumPartitions = 2 } });
    //         }
    //         catch (CreateTopicsException e)
    //         {
    //             Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
    //         }
    //     }
    // }


    // static async Task Main()
    // {
    //     var config = new AdminClientConfig
    //     {
    //         BootstrapServers = "192.168.3.158:19092"
    //     };
    //
    //     using (var adminClient = new AdminClientBuilder(config).Build())
    //     {
    //         var groups = adminClient.ListGroups(TimeSpan.FromSeconds(10));
    //         foreach (var item in groups)
    //         {
    //             Console.WriteLine(item.Group);
    //         }
    //     }
    // }

    // static async Task Main()
    // {
    //     var config = new ProducerConfig
    //     {
    //         BootstrapServers = "192.168.3.156:9092"
    //     };
    //
    //     using (var producer = new ProducerBuilder<Null, string>(config).Build())
    //     {
    //         for (int i = 0; i < 10; ++i)
    //         {
    //             producer.Produce("my-topic", new Message<Null, string> { Value = i.ToString() }, handler);
    //         }
    //     }
    //     // 帮忙程序自动退出
    //     Console.ReadKey();
    // }
    //
    // public static void handler(DeliveryReport<Null, string> r)
    // {
    //     Console.WriteLine(!r.Error.IsError
    //         ? $"Delivered message to {r.TopicPartitionOffset}"
    //         : $"Delivery Error: {r.Error.Reason}");
    // }
    //


    // static async Task Main()
    // {
    //     var config = new ProducerConfig
    //     {
    //         BootstrapServers = "192.168.3.156:9092"
    //     };
    //
    //     using (var producer = new ProducerBuilder<Null, string>(config).Build())
    //     {
    //         for (int i = 0; i < 10; ++i)
    //         {
    //             producer.Produce("my-topic", new Message<Null, string> { Value = i.ToString() }, handler);
    //         }
    //         // 只等待 10s
    //         var count = producer.Flush(TimeSpan.FromSeconds(10));
    //         // 或者使用
    //         // void Flush(CancellationToken cancellationToken = default(CancellationToken));
    //     }
    //     // 不让程序自动退出
    //     Console.ReadKey();
    // }
    //
    // public static void handler(DeliveryReport<Null, string> r)
    // {
    //     Console.WriteLine(!r.Error.IsError
    //         ? $"Delivered message to {r.TopicPartitionOffset}"
    //         : $"Delivery Error: {r.Error.Reason}");
    // }
}