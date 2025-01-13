using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Mvc;

namespace MQ.Controllers;

[ApiController]
[Route("[controller]")]
public class Demo2Controller : ControllerBase
{
    private const string SERVERS =
        "127.0.0.1:9093,127.0.0.1:19093,127.0.0.1:29093,127.0.0.1:39093,127.0.0.1:49093";

    private const string TOPIC1 = "demo2.1";
    private const string TOPIC2 = "demo2.2";

    [HttpGet]
    [Route("one-group-two-clients")]
    public async Task<object> OneGroupTwoClients()
    {
        //var config = new AdminClientConfig
        //{
        //    BootstrapServers = SERVERS
        //};
        //using (var adminClient = new AdminClientBuilder(config).Build())
        //{
        //    try
        //    {
        //        await adminClient.CreateTopicsAsync(new TopicSpecification[] {
        //            new() { Name = TOPIC1, ReplicationFactor = -1, NumPartitions = -1 } });
        //    }
        //    catch (CreateTopicsException e)
        //    {
        //        Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        //    }
        //}

        var group = "group.1";

        var consumer1 = new Thread(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = SERVERS,
                GroupId = group,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                ClientId = "a"
            };

            var builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter()); //设置反序列化方式
            var consumer = builder.Build();
            consumer.Subscribe(TOPIC1); //订阅消息使用Subscribe方法
            //consumer.Assign(new TopicPartition(topic, new Partition(0))); //从指定的Partition订阅消息使用Assign方法

            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"{config.GroupId}-{config.ClientId} receive message:{result.Message.Value}");
                consumer.Commit(result); //手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
            }
        });

        var consumer2 = new Thread(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = SERVERS,
                GroupId = group,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                ClientId = "b"
            };

            var builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter()); //设置反序列化方式
            var consumer = builder.Build();
            consumer.Subscribe(TOPIC1); //订阅消息使用Subscribe方法
            //consumer.Assign(new TopicPartition(topic, new Partition(0))); //从指定的Partition订阅消息使用Assign方法

            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"{config.GroupId}-{config.ClientId} receive message:{result.Message.Value}");
                consumer.Commit(result); //手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
            }
        });

        new Thread(() =>
        {
            var index = 0;
            var config = new ProducerConfig
            {
                BootstrapServers = SERVERS
            };

            var builder = new ProducerBuilder<string, object>(config);
            builder.SetValueSerializer(new KafkaConverter()); //设置序列化方式
            var producer = builder.Build();
            while (true)
            {
                producer.Produce(
                    TOPIC1,
                    new Message<string, object> { Key = "", Value = index });

                index++;

                Thread.Sleep(1000);
            }
        }).Start();

        consumer1.Start();
        consumer2.Start();

        while (true)
        {
            
        }

        return Ok();
    }

    [HttpGet]
    [Route("two-group-two-clients")]
    public async Task<object> TwoGroupTwoClients()
    {
        //var config = new AdminClientConfig
        //{
        //    BootstrapServers = SERVERS
        //};
        //using (var adminClient = new AdminClientBuilder(config).Build())
        //{
        //    try
        //    {
        //        await adminClient.CreateTopicsAsync(new TopicSpecification[] {
        //            new() { Name = TOPIC1, ReplicationFactor = -1, NumPartitions = -1 } });
        //    }
        //    catch (CreateTopicsException e)
        //    {
        //        Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        //    }
        //}

        var group1 = "group.1";
        var group2 = "group.2";

        var consumer1 = new Thread(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = SERVERS,
                GroupId = group1,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                ClientId = "a"
            };

            var builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter()); //设置反序列化方式
            var consumer = builder.Build();
            consumer.Subscribe(TOPIC1); //订阅消息使用Subscribe方法
            //consumer.Assign(new TopicPartition(topic, new Partition(0))); //从指定的Partition订阅消息使用Assign方法

            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"{config.GroupId}-{config.ClientId} receive message:{result.Message.Value}");
                consumer.Commit(result); //手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
            }
        });

        var consumer2 = new Thread(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = SERVERS,
                GroupId = group2,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                ClientId = "b"
            };

            var builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter()); //设置反序列化方式
            var consumer = builder.Build();
            consumer.Subscribe(TOPIC1); //订阅消息使用Subscribe方法
            //consumer.Assign(new TopicPartition(topic, new Partition(0))); //从指定的Partition订阅消息使用Assign方法

            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"{config.GroupId}-{config.ClientId} receive message:{result.Message.Value}");
                consumer.Commit(result); //手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
            }
        });

        new Thread(() =>
        {
            var index = 0;
            var config = new ProducerConfig
            {
                BootstrapServers = SERVERS
            };

            var builder = new ProducerBuilder<string, object>(config);
            builder.SetValueSerializer(new KafkaConverter()); //设置序列化方式
            var producer = builder.Build();
            while (true)
            {
                producer.Produce(
                    TOPIC1,
                    new Message<string, object> { Key = "", Value = index });

                index++;

                Thread.Sleep(1000);
            }
        }).Start();

        consumer1.Start();
        consumer2.Start();

        while (true)
        {

        }

        return Ok();
    }

    [HttpGet]
    [Route("one-group-two-partitions")]
    public async Task<object> OneGroupTwoClientPartitions()
    {
        var group = "group.1";

        var consumer1 = new Thread(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = SERVERS,
                GroupId = group,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                ClientId = "a"
            };

            var builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter()); //设置反序列化方式
            var consumer = builder.Build();
            consumer.Assign(new TopicPartition(TOPIC2, new Partition(0))); //从指定的Partition订阅消息使用Assign方法

            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"{config.GroupId}-{config.ClientId} receive message:{result.Message.Value}");
                consumer.Commit(result); //手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
            }
        });

        var consumer2 = new Thread(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = SERVERS,
                GroupId = group,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                ClientId = "b"
            };

            var builder = new ConsumerBuilder<string, object>(config);
            builder.SetValueDeserializer(new KafkaConverter()); //设置反序列化方式
            var consumer = builder.Build();
            consumer.Assign(new TopicPartition(TOPIC2, new Partition(1))); //从指定的Partition订阅消息使用Assign方法

            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"{config.GroupId}-{config.ClientId} receive message:{result.Message.Value}");
                consumer.Commit(result); //手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
            }
        });

        new Thread(() =>
        {
            var index = 0;
            var config = new ProducerConfig
            {
                BootstrapServers = SERVERS
            };

            var builder = new ProducerBuilder<string, object>(config);
            builder.SetValueSerializer(new KafkaConverter()); //设置序列化方式
            var producer = builder.Build();
            while (true)
            {
                int partition = index % 2;
                var topicPartition = new TopicPartition(TOPIC2, new Partition(partition));

                producer.Produce(
                    topicPartition,
                    new Message<string, object> { Key = "", Value = index });

                index++;

                Thread.Sleep(1000);
            }
        }).Start();

        consumer1.Start();
        consumer2.Start();

        while (true)
        {

        }

        return Ok();
    }
}