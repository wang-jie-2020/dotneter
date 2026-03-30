using Kafka_WorkPatterns;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Kafka 工作模式和确认机制演示");
        Console.WriteLine("==============================");

        var kafkaDemo = new KafkaWorkPatterns();
        string testTopic = "kafka-demo-topic";
        string testTopic2 = "kafka-demo-topic-2";
        string testTopic3 = "kafka-demo-topic-3";

        // 演示简单生产者和消费者
        Console.WriteLine("\n1. 简单生产者和消费者演示:");
        Console.WriteLine("启动消费者...");

        // 启动消费者线程
        Task.Run(() => kafkaDemo.SimpleConsumer(testTopic, "group-1"));

        // 等待消费者启动
        Thread.Sleep(2000);

        Console.WriteLine("发送 10 条消息...");
        kafkaDemo.SimpleProducer(testTopic, 10);

        // 等待消息处理
        Thread.Sleep(5000);

        // 演示分区生产者
        Console.WriteLine("\n2. 分区生产者演示:");
        Console.WriteLine("发送消息到不同分区...");
        kafkaDemo.PartitionProducer(testTopic, 5);

        // 等待消息处理
        Thread.Sleep(3000);

        // 演示多主题消费者
        Console.WriteLine("\n3. 多主题消费者演示:");
        Console.WriteLine("启动多主题消费者...");

        var topics = new List<string> { testTopic, testTopic2, testTopic3 };
        Task.Run(() => kafkaDemo.MultiTopicConsumer(topics, "group-2"));

        // 等待消费者启动
        Thread.Sleep(2000);

        Console.WriteLine("向多个主题发送消息...");
        kafkaDemo.SimpleProducer(testTopic2, 3);
        kafkaDemo.SimpleProducer(testTopic3, 3);

        // 等待消息处理
        Thread.Sleep(3000);

        // 演示事务生产者
        Console.WriteLine("\n4. 事务生产者演示:");
        Console.WriteLine("发送事务消息...");
        kafkaDemo.TransactionProducer(testTopic, 5);

        // 等待消息处理
        Thread.Sleep(3000);

        // 演示精确一次处理
        Console.WriteLine("\n5. 精确一次处理演示:");
        Console.WriteLine("启动精确一次处理...");

        string inputTopic = "exactly-once-input";
        string outputTopic = "exactly-once-output";

        Task.Run(() => kafkaDemo.ExactlyOnceProcessing(inputTopic, outputTopic, "group-3"));

        // 等待处理程序启动
        Thread.Sleep(2000);

        Console.WriteLine("向输入主题发送消息...");
        kafkaDemo.SimpleProducer(inputTopic, 5);

        // 等待消息处理
        Thread.Sleep(5000);

        // 演示异步生产者
        Console.WriteLine("\n6. 异步生产者演示:");
        Console.WriteLine("异步发送消息...");
        kafkaDemo.AsyncProducer(testTopic, 5).Wait();

        // 等待消息处理
        Thread.Sleep(3000);

        // 演示带消息头的生产者和消费者
        Console.WriteLine("\n7. 带消息头的消息演示:");
        Console.WriteLine("启动带消息头的消费者...");

        Task.Run(() => kafkaDemo.ConsumerWithHeaders(testTopic, "group-4"));

        // 等待消费者启动
        Thread.Sleep(2000);

        Console.WriteLine("发送带消息头的消息...");
        kafkaDemo.ProducerWithHeaders(testTopic, 5);

        // 等待消息处理
        Thread.Sleep(5000);

        Console.WriteLine("\n演示完成！按任意键退出...");
        Console.ReadKey();
    }
}
