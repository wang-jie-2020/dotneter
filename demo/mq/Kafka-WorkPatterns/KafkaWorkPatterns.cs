using Confluent.Kafka;
using System.Text.Json;

namespace Kafka_WorkPatterns
{
    public class KafkaWorkPatterns
    {
        private readonly string _bootstrapServers = "127.0.0.1:9092";

        // 简单生产者 - 基本的消息发送
        public void SimpleProducer(string topic, int messageCount = 10)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                Acks = Acks.All // 等待所有副本确认
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Simple message {i}";
                    var key = $"key-{i}";

                    var deliveryReport = producer.ProduceAsync(topic, new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    }).Result;

                    Console.WriteLine($"[Simple Producer] Sent: {message}, Partition: {deliveryReport.Partition}, Offset: {deliveryReport.Offset}");
                }

                // 刷新所有待处理的消息
                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        // 简单消费者 - 基本的消息消费
        public void SimpleConsumer(string topic, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false // 手动提交偏移量
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(topic);
                Console.WriteLine($"[Simple Consumer] Started, waiting for messages...");

                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(1));
                        if (consumeResult != null)
                        {
                            Console.WriteLine($"[Simple Consumer] Received: {consumeResult.Message.Value}, Key: {consumeResult.Message.Key}, Partition: {consumeResult.Partition}, Offset: {consumeResult.Offset}");
                            // 手动提交偏移量
                            consumer.Commit(consumeResult);
                            Console.WriteLine($"[Simple Consumer] Committed offset: {consumeResult.Offset}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }

        // 分区生产者 - 发送到指定分区
        public void PartitionProducer(string topic, int messageCount = 5)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Partition message {i}";
                    var key = $"key-{i}";
                    var partition = i % 3; // 发送到不同的分区

                    var deliveryReport = producer.ProduceAsync(new TopicPartition(topic, new Partition(partition)), new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    }).Result;

                    Console.WriteLine($"[Partition Producer] Sent: {message}, Partition: {deliveryReport.Partition}, Offset: {deliveryReport.Offset}");
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        // 多主题消费者 - 消费多个主题
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
                Console.WriteLine($"[Multi-Topic Consumer] Started, subscribed to topics: {string.Join(", ", topics)}");

                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(1));
                        if (consumeResult != null)
                        {
                            Console.WriteLine($"[Multi-Topic Consumer] Topic: {consumeResult.Topic}, Received: {consumeResult.Message.Value}, Key: {consumeResult.Message.Key}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }

        // 事务生产者 - 支持事务
        public void TransactionProducer(string topic, int messageCount = 5)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                TransactionalId = "transactional-producer-1"
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                // 初始化事务
                producer.InitTransactions(TimeSpan.FromSeconds(10));

                try
                {
                    // 开始事务
                    producer.BeginTransaction();

                    for (int i = 0; i < messageCount; i++)
                    {
                        var message = $"Transactional message {i}";
                        var key = $"tx-key-{i}";

                        producer.Produce(topic, new Message<string, string>
                        {
                            Key = key,
                            Value = message
                        });

                        Console.WriteLine($"[Transaction Producer] Sent: {message}");
                    }

                    // 提交事务
                    producer.CommitTransaction(TimeSpan.FromSeconds(10));
                    Console.WriteLine("[Transaction Producer] Transaction committed");
                }
                catch (Exception ex)
                {
                    // 中止事务
                    producer.AbortTransaction(TimeSpan.FromSeconds(10));
                    Console.WriteLine($"[Transaction Producer] Transaction aborted: {ex.Message}");
                }
            }
        }

        // 精确一次处理 - 结合事务和消费者偏移量提交
        public void ExactlyOnceProcessing(string inputTopic, string outputTopic, string groupId)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                TransactionalId = $"exactly-once-producer-{groupId}"
            };

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                IsolationLevel = IsolationLevel.ReadCommitted // 只读取已提交的消息
            };

            using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
            using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
            {
                consumer.Subscribe(inputTopic);
                producer.InitTransactions(TimeSpan.FromSeconds(10));

                Console.WriteLine($"[Exactly Once] Started processing from {inputTopic} to {outputTopic}");

                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(1));
                        if (consumeResult != null)
                        {
                            // 开始事务
                            producer.BeginTransaction();

                            try
                            {
                                // 处理消息
                                var processedMessage = $"Processed: {consumeResult.Message.Value}";

                                // 发送到输出主题
                                producer.Produce(outputTopic, new Message<string, string>
                                {
                                    Key = consumeResult.Message.Key,
                                    Value = processedMessage
                                });

                                // 将消费者偏移量提交到事务
                                producer.SendOffsetsToTransaction(new List<TopicPartitionOffset>
                                {
                                    new TopicPartitionOffset(consumeResult.TopicPartition, consumeResult.Offset + 1)
                                }, consumer.ConsumerGroupMetadata, TimeSpan.FromSeconds(10));

                                // 提交事务
                                producer.CommitTransaction(TimeSpan.FromSeconds(10));

                                Console.WriteLine($"[Exactly Once] Processed: {consumeResult.Message.Value} -> {processedMessage}");
                            }
                            catch (Exception ex)
                            {
                                producer.AbortTransaction(TimeSpan.FromSeconds(10));
                                Console.WriteLine($"[Exactly Once] Error processing message: {ex.Message}");
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }

        // 异步生产者 - 使用异步发送
        public async Task AsyncProducer(string topic, int messageCount = 10)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var tasks = new List<Task<DeliveryResult<string, string>>>();

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Async message {i}";
                    var key = $"async-key-{i}";

                    var task = producer.ProduceAsync(topic, new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    });

                    tasks.Add(task);
                    Console.WriteLine($"[Async Producer] Sent: {message}");
                }

                // 等待所有发送完成
                var results = await Task.WhenAll(tasks);
                foreach (var result in results)
                {
                    Console.WriteLine($"[Async Producer] Delivered: Partition={result.Partition}, Offset={result.Offset}");
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        // 带消息头的生产者
        public void ProducerWithHeaders(string topic, int messageCount = 5)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Message with headers {i}";
                    var key = $"header-key-{i}";

                    var msg = new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    };

                    // 添加自定义头信息
                    msg.Headers.Add("message-type", System.Text.Encoding.UTF8.GetBytes("important"));
                    msg.Headers.Add("priority", System.Text.Encoding.UTF8.GetBytes("high"));
                    msg.Headers.Add("timestamp", System.Text.Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString()));

                    var deliveryReport = producer.ProduceAsync(topic, msg).Result;
                    Console.WriteLine($"[Producer with Headers] Sent: {message}, Headers: message-type=important, priority=high");
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        // 消费带消息头的消息
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
                Console.WriteLine($"[Consumer with Headers] Started, waiting for messages...");

                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(1));
                        if (consumeResult != null)
                        {
                            // 提取消息头
                            var headers = new Dictionary<string, string>();
                            foreach (var header in consumeResult.Message.Headers)
                            {
                                headers[header.Key] = System.Text.Encoding.UTF8.GetString(header.GetValueBytes());
                            }

                            var headerInfo = string.Join(", ", headers.Select(h => $"{h.Key}={h.Value}"));
                            Console.WriteLine($"[Consumer with Headers] Received: {consumeResult.Message.Value}, Headers: {headerInfo}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }
    }
}
