using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka_WorkPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Kafka 核心工作特性演示");
            Console.WriteLine("==============================");
            Console.WriteLine("演示 Kafka 的核心特性：高吞吐、分区、消费者组、事务、精确一次处理\n");

            var sender = new KafkaSender();
            var consumer = new KafkaConsumer();
            
            string orderTopic = "demo-orders";
            string inventoryTopic = "demo-inventory";
            string topic2 = "demo-topic-2";
            string topic3 = "demo-topic-3";

            // 1. 高吞吐演示 - 简单生产者和消费者
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 1. 高吞吐特性演示 - 批处理和压缩                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 批处理：多条消息批量发送，提高吞吐量");
            Console.WriteLine("  - 压缩：使用 LZ4 压缩，减少网络传输");
            Console.WriteLine("  - 异步发送：非阻塞，高性能\n");

            Console.WriteLine("启动消费者...");
            Task.Run(() => consumer.SimpleConsumer(orderTopic, "order-consumer-group", 20));
            Thread.Sleep(2000);

            Console.WriteLine("\n发送 100 条订单消息...");
            sender.SimpleProducer(orderTopic, 100);
            Thread.Sleep(5000);

            // 2. 分区和顺序性演示
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 2. 分区和顺序性演示 - 分区键保证消息顺序                ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 分区键：相同 key 的消息总是发送到同一分区");
            Console.WriteLine("  - 顺序性：同一分区内的消息是有序的");
            Console.WriteLine("  - 并行性：不同分区可以并行处理\n");

            sender.PartitionProducer(orderTopic, 30);
            Thread.Sleep(2000);

            // 3. 消费者组并行消费演示
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 3. 消费者组并行消费 - 多消费者并行处理                  ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 消费者组：多个消费者实例共同消费一个主题");
            Console.WriteLine("  - 分区分配：每个分区只能被一个消费者消费");
            Console.WriteLine("  - 水平扩展：增加消费者数量提高消费能力\n");

            consumer.ParallelConsumers(orderTopic, "parallel-consumer-group", 3);
            Thread.Sleep(2000);

            // 4. 多主题消费演示
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 4. 多主题消费 - 一个消费者订阅多个主题                  ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 多主题订阅：一个消费者可订阅多个主题");
            Console.WriteLine("  - 主题模式匹配：支持正则表达式订阅\n");

            var topics = new List<string> { orderTopic, topic2, topic3 };
            Task.Run(() => consumer.MultiTopicConsumer(topics, "multi-topic-group"));
            Thread.Sleep(2000);

            Console.WriteLine("\n向多个主题发送消息...");
            sender.SimpleProducer(topic2, 10);
            sender.SimpleProducer(topic3, 10);
            Thread.Sleep(5000);

            // 5. 事务和 Exactly-Once 演示
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 5. 事务和 Exactly-Once - 跨主题原子操作                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 事务：跨分区的原子操作");
            Console.WriteLine("  - 幂等性：防止消息重复");
            Console.WriteLine("  - Exactly-Once：端到端的精确一次处理\n");

            string inputTopic = "exactly-once-input";
            string outputTopic = "exactly-once-output";

            Task.Run(() => consumer.ExactlyOnceProcessing(inputTopic, outputTopic, "exactly-once-group"));
            Thread.Sleep(2000);

            Console.WriteLine("\n向输入主题发送 10 条消息...");
            sender.SimpleProducer(inputTopic, 10);
            Thread.Sleep(8000);

            // 6. 异步高性能演示
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 6. 异步高性能 - 非阻塞发送                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 异步非阻塞：提高并发性能");
            Console.WriteLine("  - 回调确认：消息发送结果通知");
            Console.WriteLine("  - 低延迟：微秒级延迟\n");

            sender.AsyncProducer(orderTopic, 50).Wait();
            Thread.Sleep(3000);

            // 7. 消息头演示
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 7. 消息头 - 元数据传递和链路追踪                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 消息头：传递业务上下文信息");
            Console.WriteLine("  - 链路追踪：trace-id 用于分布式追踪");
            Console.WriteLine("  - 业务元数据：用户 ID、时间戳、来源等\n");

            Task.Run(() => consumer.ConsumerWithHeaders(orderTopic, "headers-consumer-group"));
            Thread.Sleep(2000);

            Console.WriteLine("\n发送带消息头的消息...");
            sender.ProducerWithHeaders(orderTopic, 10);
            Thread.Sleep(5000);

            // 8. 分区感知消费者演示
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 8. 分区感知 - 分区分配和重平衡                          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝\n");
            Console.WriteLine("特性说明：");
            Console.WriteLine("  - 分区分配：消费者加入时的分区分配策略");
            Console.WriteLine("  - 重平衡：消费者组成员变化时的分区重新分配");
            Console.WriteLine("  - CooperativeSticky：协作式粘性分配策略\n");

            consumer.PartitionAwareConsumer(orderTopic, "partition-aware-group");
            Thread.Sleep(5000);

            // 结束
            Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║ 演示完成！                                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
