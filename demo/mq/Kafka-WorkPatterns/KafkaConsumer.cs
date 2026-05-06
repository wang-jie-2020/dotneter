using System.Diagnostics;
using Confluent.Kafka;

namespace Kafka_WorkPatterns
{
    public class KafkaConsumer
    {
        private readonly string _bootstrapServers = "127.0.0.1:9093,127.0.0.1:19093,127.0.0.1:29093,127.0.0.1:39093,127.0.0.1:49093";

        /// <summary>
        /// 简单消费者 - 演示基本的消息消费和手动提交
        /// Kafka 特性：消费者组、手动提交偏移量、至少一次投递
        /// </summary>
        public void SimpleConsumer(string topic, string groupId, int maxMessages = 20)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false, // 手动提交
                EnablePartitionEof = true // 检测分区结束
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(topic);
                Console.WriteLine($"[Simple Consumer] 消费者组 '{groupId}' 已启动，订阅主题：{topic}");
                Console.WriteLine($"[Simple Consumer] 等待接收最多 {maxMessages} 条消息...\n");

                int messageCount = 0;
                try
                {
                    while (messageCount < maxMessages)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(2));
                        if (consumeResult != null)
                        {
                            // 模拟业务处理
                            Thread.Sleep(100);

                            Console.WriteLine($"[Simple Consumer] [组:{groupId}] 分区{consumeResult.Partition},偏移量{consumeResult.Offset},键{consumeResult.Message.Key},值:{consumeResult.Message.Value}");

                            // 手动提交偏移量 - 确保消息至少处理一次
                            consumer.Commit(consumeResult);
                            messageCount++;
                        }
                        else if (consumeResult?.IsPartitionEOF == true)
                        {
                            Console.WriteLine($"[Simple Consumer] 已到达分区 {consumeResult.Partition} 的末尾");
                        }
                    }

                    Console.WriteLine($"[Simple Consumer] [组:{groupId}] 已处理 {messageCount} 条消息，准备退出\n");
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"[Simple Consumer] 消费错误：{ex.Error.Reason}");
                }
                finally
                {
                    consumer.Close();
                    Console.WriteLine($"[Simple Consumer] [组:{groupId}] 消费者已关闭\n");
                }
            }
        }

        /// <summary>
        /// 多消费者并行消费 - 演示消费者组的并行处理能力
        /// Kafka 特性：一个分区只能被一个消费者消费，多个消费者可并行消费不同分区
        /// </summary>
        public void ParallelConsumers(string topic, string groupId, int consumerCount = 3)
        {
            Console.WriteLine($"\n[Parallel Consumers] 启动 {consumerCount} 个消费者，消费者组：{groupId}");
            Console.WriteLine($"[Parallel Consumers] 主题：{topic}\n");

            var consumers = new List<Task>();
            var cts = new CancellationTokenSource();

            // 启动多个消费者实例
            for (int i = 0; i < consumerCount; i++)
            {
                int consumerIndex = i;
                consumers.Add(Task.Run(() =>
                {
                    var config = new ConsumerConfig
                    {
                        BootstrapServers = _bootstrapServers,
                        GroupId = groupId,
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnableAutoCommit = true
                    };

                    using (var consumer = new ConsumerBuilder<string, string>(config).Build())
                    {
                        consumer.Subscribe(topic);
                        Console.WriteLine($"[Parallel Consumers] 消费者 #{consumerIndex} 已启动");

                        int messageCount = 0;
                        var startTime = Stopwatch.StartNew();

                        try
                        {
                            while (!cts.Token.IsCancellationRequested && messageCount < 10)
                            {
                                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(1));
                                if (consumeResult != null)
                                {
                                    messageCount++;
                                    Console.WriteLine($"[Parallel Consumers] 消费者#{consumerIndex} | 分区{consumeResult.Partition} | 消息:{consumeResult.Message.Value}");
                                }
                            }

                            if (messageCount > 0)
                            {
                                startTime.Stop();
                                Console.WriteLine($"[Parallel Consumers] 消费者#{consumerIndex} 处理完成：{messageCount}条消息，耗时{startTime.ElapsedMilliseconds}ms");
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine($"[Parallel Consumers] 消费者#{consumerIndex} 已取消");
                        }
                        finally
                        {
                            consumer.Close();
                        }
                    }
                }));
            }

            // 等待所有消费者完成或超时
            Task.WaitAll(consumers.ToArray(), TimeSpan.FromSeconds(15));
            cts.Cancel();

            Console.WriteLine($"\n[Parallel Consumers] 所有消费者已完成或超时\n");
        }

        /// <summary>
        /// 多主题消费者 - 演示订阅多个主题
        /// Kafka 特性：一个消费者可订阅多个主题
        /// </summary>
        public void MultiTopicConsumer(List<string> topics, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(topics);
                Console.WriteLine($"[Multi-Topic Consumer] 已启动，订阅主题：[{string.Join(", ", topics)}]");
                Console.WriteLine($"[Multi-Topic Consumer] 消费者组：{groupId}\n");

                try
                {
                    int messageCount = 0;
                    while (messageCount < 30)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(2));
                        if (consumeResult != null)
                        {
                            messageCount++;
                            Console.WriteLine($"[Multi-Topic Consumer] 主题:{consumeResult.Topic,-20} 分区{consumeResult.Partition} 消息:{consumeResult.Message.Value}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("[Multi-Topic Consumer] 消费者已取消");
                }
                finally
                {
                    consumer.Close();
                    Console.WriteLine("\n[Multi-Topic Consumer] 消费者已关闭\n");
                }
            }
        }

        /// <summary>
        /// 精确一次处理 - 演示 Exactly-Once 语义
        /// Kafka 特性：事务 + 偏移量提交，保证端到端的精确一次处理
        /// </summary>
        public void ExactlyOnceProcessing(string inputTopic, string outputTopic, string groupId)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                TransactionalId = $"exactly-once-{groupId}-{Guid.NewGuid():N}",
                EnableIdempotence = true
            };

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                IsolationLevel = IsolationLevel.ReadCommitted // 只读已提交的消息
            };

            using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                consumer.Subscribe(inputTopic);
                producer.InitTransactions(TimeSpan.FromSeconds(10));

                Console.WriteLine($"[Exactly Once] 精确一次处理已启动");
                Console.WriteLine($"[Exactly Once] 输入主题：{inputTopic}, 输出主题：{outputTopic}\n");

                int processedCount = 0;
                try
                {
                    while (processedCount < 10)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(2));
                        if (consumeResult != null)
                        {
                            try
                            {
                                // 开始事务
                                producer.BeginTransaction();

                                // 业务处理
                                var processedMessage = $"[已处理]{consumeResult.Message.Value}";

                                // 发送到输出主题
                                producer.Produce(outputTopic, new Message<string, string>
                                {
                                    Key = consumeResult.Message.Key,
                                    Value = processedMessage
                                });

                                // 将偏移量提交到事务中 - 关键步骤
                                producer.SendOffsetsToTransaction(
                                    new List<TopicPartitionOffset>
                                    {
                                        new TopicPartitionOffset(consumeResult.TopicPartition, consumeResult.Offset + 1)
                                    },
                                    consumer.ConsumerGroupMetadata,
                                    TimeSpan.FromSeconds(10)
                                );

                                // 提交事务 - 原子性地提交偏移量和发送消息
                                producer.CommitTransaction(TimeSpan.FromSeconds(10));

                                processedCount++;
                                Console.WriteLine($"[Exactly Once] ✓ 处理 #{processedCount}: {consumeResult.Message.Value} -> {processedMessage}");
                            }
                            catch (Exception ex)
                            {
                                producer.AbortTransaction(TimeSpan.FromSeconds(10));
                                Console.WriteLine($"[Exactly Once] ✗ 事务回滚：{ex.Message}");
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("[Exactly Once] 处理已取消");
                }
                finally
                {
                    consumer.Close();
                    Console.WriteLine($"\n[Exactly Once] 处理完成，共处理 {processedCount} 条消息\n");
                }
            }
        }

        /// <summary>
        /// 消费带消息头的消息 - 演示读取消息元数据
        /// Kafka 特性：消息头支持，用于链路追踪、业务上下文等
        /// </summary>
        public void ConsumerWithHeaders(string topic, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(topic);
                Console.WriteLine($"[Consumer with Headers] 已启动，订阅主题：{topic}");
                Console.WriteLine($"[Consumer with Headers] 消费者组：{groupId}\n");

                try
                {
                    int messageCount = 0;
                    while (messageCount < 10)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(2));
                        if (consumeResult != null)
                        {
                            messageCount++;

                            // 提取消息头
                            var headers = new Dictionary<string, string>();
                            foreach (var header in consumeResult.Message.Headers)
                            {
                                string value = header.GetValueBytes() != null 
                                    ? System.Text.Encoding.UTF8.GetString(header.GetValueBytes()) 
                                    : "null";
                                headers[header.Key] = value;
                            }

                            // 打印消息和头信息
                            Console.WriteLine($"[Consumer with Headers] 消息 #{messageCount}");
                            Console.WriteLine($"  主题：{consumeResult.Topic}, 分区：{consumeResult.Partition}");
                            Console.WriteLine($"  键：{consumeResult.Message.Key}, 值：{consumeResult.Message.Value}");
                            
                            if (headers.Count > 0)
                            {
                                Console.WriteLine($"  消息头:");
                                foreach (var header in headers)
                                {
                                    Console.WriteLine($"    {header.Key}: {header.Value}");
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("[Consumer with Headers] 消费者已取消");
                }
                finally
                {
                    consumer.Close();
                    Console.WriteLine("\n[Consumer with Headers] 消费者已关闭\n");
                }
            }
        }

        /// <summary>
        /// 分区感知消费者 - 演示分区消费
        /// Kafka 特性：分区消费、偏移量管理
        /// </summary>
        public void PartitionAwareConsumer(string topic, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(topic);
                Console.WriteLine($"[Partition Aware] 消费者已启动，组：{groupId}");
                Console.WriteLine($"[Partition Aware] 订阅主题：{topic}\n");

                try
                {
                    int messageCount = 0;
                    while (messageCount < 15)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(2));
                        if (consumeResult != null)
                        {
                            messageCount++;
                            Console.WriteLine($"[Partition Aware] 分区{consumeResult.Partition},偏移量{consumeResult.Offset}: {consumeResult.Message.Value}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("[Partition Aware] 消费者已取消");
                }
                finally
                {
                    consumer.Close();
                    Console.WriteLine("\n[Partition Aware] 消费者已关闭\n");
                }
            }
        }
    }
}
