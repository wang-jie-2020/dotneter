using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace MQ.Controllers;

[ApiController]
[Route("[Controller]")]
public class Demo1Controller : ControllerBase
{
    private const string SERVERS =
        "127.0.0.1:9093,127.0.0.1:19093,127.0.0.1:29093,127.0.0.1:39093,127.0.0.1:49093";

    private const string TOPIC = "demo1";

    [HttpGet]
    [Route("one-producer")]
    public async Task<object> OneProducer(string message = "hello world")
    {
        //1、消息发布需要使用生产者对象，它由ProducerBuilder<,>类构造，有两个泛型参数，第一个是路由Key的类型，第二个是消息的类型，开发过程中，我们多数使用ProducerBuilder<string, object>或者ProducerBuilder<string, string>。
        //2、ProducerBuilder<string, object>在实例化时需要一个配置参数，这个配置参数是一个集合（IEnumerable<KeyValuePair<string, string>>），ProducerConfig其实是实现了这个集合接口的一个类型，在旧版本的Confluent.Kafka中，是没有这个ProducerConfig类型的，之前都是使用Dictionary<string,string>来构建ProducerBuilder<string, object>
        var config = new ProducerConfig
        {
            BootstrapServers = SERVERS
        };

        // 3、Confluent.Kafka还要求提供一个实现了ISerializer<TValue>或者IAsyncSerializer<TValue>接口的序列化类型，比如上面的Demo中的KafkaConverter：　　
        var builder = new ProducerBuilder<string, object>(config);
        builder.SetValueSerializer(new KafkaConverter());

        // 4、发布消息使用Produce方法，它有几个重载，还有几个异步发布方法。
        // 第一个参数是topic，如果想指定Partition，需要使用TopicPartition对象，
        // 第二个参数是消息，它是Message<TKey, TValue>类型，Key即路由，Value就是我们的消息，消息会经过ISerializer<TValue>接口序列化之后发送到Kafka，
        // 第三个参数是Action<DeliveryReport<TKey, TValue>>类型的委托，它是异步执行的，其实是发布的结果通知。
        var producer = builder.Build();
        producer.Produce(
            TOPIC,
            new Message<string, object> { Key = "hi", Value = message });
        return Ok();
    }

    [HttpGet]
    [Route("one-consumer")]
    public async Task<object> OneConsumer()
    {
        //1、和消息发布一样，消费者的构建是通过ConsumerBuilder<, >对象来完成的，同样也有一个ConsumerConfig配置对象，它在旧版本中也是不存在的，旧版本中也是使用Dictionary<string,string>来实现的
        var config = new ConsumerConfig
        {
            BootstrapServers = SERVERS,
            GroupId = "group.1", //消费者的Group，注意了，Kafka以Group的形式消费消息，一个消息只会被同一Group中的一个消费者消费，另外，一般的，同一Group中的消费者应该实现相同的逻辑
            AutoOffsetReset =
                AutoOffsetReset
                    .Earliest, //自动重置offset的行为，默认是Latest，这是kafka读取数据的策略，有三个可选值：Latest，Earliest，Error，个人推荐使用Earliest
            EnableAutoCommit = false //是否自动提交，如果设置成true，那么消费者接收到消息就相当于被消费了，我们可以设置成false，然后在我们处理完逻辑之后手动提交。
        };

        /*
            自动重置offset的行为:
            Latest：当各分区下有已提交的offset时，从提交的offset开始消费；无提交的offset时，消费新产生的该分区下的数据
　　         Earliest：当各分区下有已提交的offset时，从提交的offset开始消费；无提交的offset时，从头开始消费
　　         Error：topic各分区都存在已提交的offset时，从offset后开始消费；只要有一个分区不存在已提交的offset，则抛出异常
         */

        var builder = new ConsumerBuilder<string, object>(config);
        builder.SetValueDeserializer(new KafkaConverter()); //设置反序列化方式
        var consumer = builder.Build();

        // Subscribe：从一个或者多个topic订阅消息
        // Assign：从一个或者多个topic的指定partition中订阅消息
        // 另外，Confluent.Kafka还提供了两个取消订阅的方法：Unsubscribe和Unassign

        consumer.Subscribe(TOPIC); //订阅消息使用Subscribe方法
        //consumer.Assign(new TopicPartition("test", new Partition(1)));//从指定的Partition订阅消息使用Assign方法

        while (true)
        {
            var result = consumer.Consume();
            Console.WriteLine($"receive message:{result.Message.Key},{result.Message.Value}");
            consumer.Commit(result); //手动提交，如果上面的EnableAutoCommit=true表示自动提交，则无需调用Commit方法
        }

        return Ok();
    }
}