using System.Diagnostics;
using Confluent.Kafka;

namespace Kafka_WorkPatterns
{
    public class KafkaSender
    {
        private readonly string _bootstrapServers = "127.0.0.1:9093,127.0.0.1:19093,127.0.0.1:29093,127.0.0.1:39093,127.0.0.1:49093";

        /// <summary>
        /// 简单生产者 - 演示基本的消息发送
        /// Kafka 特性：高吞吐、异步批处理
        /// </summary>
        public void SimpleProducer(string topic, int messageCount = 100)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                // 批处理配置 - 提高吞吐量
                LingerMs = 5,           // 等待批处理的时间
                BatchNumMessages = 10000, // 批处理消息数量
                CompressionType = CompressionType.Lz4    // 压缩类型
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                int successCount = 0;
                var startTime = Stopwatch.StartNew();

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"order-{DateTime.Now:yyyyMMddHHmmss}-{i}";
                    var key = $"user-{i % 10}"; // 10 个用户

                    producer.Produce(topic, new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    }, deliveryReport =>
                    {
                        if (deliveryReport.Error.Code == ErrorCode.NoError)
                        {
                            successCount++;
                        }
                    });
                }

                // 等待所有消息发送完成
                producer.Flush(TimeSpan.FromSeconds(10));
                startTime.Stop();

                Console.WriteLine($"[Simple Producer] 发送完成：成功 {successCount}/{messageCount} 条消息，耗时 {startTime.ElapsedMilliseconds}ms");
                Console.WriteLine($"[Simple Producer] 吞吐量：{messageCount * 1000.0 / startTime.ElapsedMilliseconds:F2} 消息/秒");
            }
        }

        /// <summary>
        /// 分区生产者 - 演示分区键的使用
        /// Kafka 特性：相同 key 的消息总是发送到同一分区，保证顺序性
        /// </summary>
        public void PartitionProducer(string topic, int messageCount = 20)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                // 模拟 3 个用户的订单，每个用户 10 条订单
                for (int userId = 0; userId < 3; userId++)
                {
                    string userKey = $"user-{userId}";
                    
                    Console.WriteLine($"\n[Partition Producer] === 用户 {userId} 的订单 ===");
                    
                    for (int i = 0; i < messageCount / 3; i++)
                    {
                        var message = $"Order-{userId}-{i}";
                        
                        var deliveryReport = producer.ProduceAsync(topic, new Message<string, string>
                        {
                            Key = userKey,  // 使用用户 ID 作为分区键
                            Value = message
                        }).Result;

                        Console.WriteLine($"[Partition Producer] 用户{userId} -> 分区{deliveryReport.Partition}, 偏移量{deliveryReport.Offset}, 消息：{message}");
                    }
                }

                producer.Flush(TimeSpan.FromSeconds(10));
                Console.WriteLine("\n[Partition Producer] 提示：相同用户的订单总是发送到同一分区，保证了订单的顺序性");
            }
        }

        /// <summary>
        /// 事务生产者 - 演示 Exactly-Once 语义
        /// Kafka 特性：跨分区事务，保证原子性
        /// </summary>
        public void TransactionProducer(string ordersTopic, string inventoryTopic, int orderCount = 5)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                TransactionalId = $"order-tx-producer-{Guid.NewGuid()}",
                // 启用幂等性，防止重复
                EnableIdempotence = true
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                producer.InitTransactions(TimeSpan.FromSeconds(10));

                for (int i = 0; i < orderCount; i++)
                {
                    var orderId = $"ORDER-{DateTime.Now:yyyyMMdd}-{i:D4}";
                    var userId = $"USER-{i % 3}";

                    try
                    {
                        // 开始事务
                        producer.BeginTransaction();
                        Console.WriteLine($"\n[Transaction Producer] 开始事务：{orderId}");

                        // 1. 发送订单消息
                        var orderMessage = $"创建订单:{orderId},用户:{userId},金额:{(i + 1) * 100}";
                        producer.Produce(ordersTopic, new Message<string, string>
                        {
                            Key = userId,
                            Value = orderMessage
                        });
                        Console.WriteLine($"[Transaction Producer] 订单消息：{orderMessage}");

                        // 2. 发送库存扣减消息
                        var inventoryMessage = $"扣减库存:订单{orderId},商品 SKU-{i % 5},数量:1";
                        producer.Produce(inventoryTopic, new Message<string, string>
                        {
                            Key = $"SKU-{i % 5}",
                            Value = inventoryMessage
                        });
                        Console.WriteLine($"[Transaction Producer] 库存消息：{inventoryMessage}");

                        // 提交事务 - 两个消息要么都成功，要么都失败
                        producer.CommitTransaction(TimeSpan.FromSeconds(10));
                        Console.WriteLine($"[Transaction Producer] ✓ 事务提交成功：{orderId}");
                    }
                    catch (Exception ex)
                    {
                        producer.AbortTransaction(TimeSpan.FromSeconds(10));
                        Console.WriteLine($"[Transaction Producer] ✗ 事务回滚：{orderId}, 错误：{ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 异步生产者 - 演示回调确认机制
        /// Kafka 特性：异步非阻塞，高性能
        /// </summary>
        public async Task AsyncProducer(string topic, int messageCount = 50)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var tasks = new List<Task<DeliveryResult<string, string>>>();
                var startTime = Stopwatch.StartNew();

                Console.WriteLine($"\n[Async Producer] 开始异步发送 {messageCount} 条消息...");

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"async-msg-{i}";
                    var key = $"async-key-{i % 5}";

                    var task = producer.ProduceAsync(topic, new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    });

                    tasks.Add(task);
                }

                // 等待所有发送完成
                var results = await Task.WhenAll(tasks);
                startTime.Stop();

                var successCount = results.Count(r => r.Status == PersistenceStatus.NotPersisted || r.Status == PersistenceStatus.Persisted);
                Console.WriteLine($"[Async Producer] 发送完成：成功 {successCount}/{messageCount} 条，耗时 {startTime.ElapsedMilliseconds}ms");
                Console.WriteLine($"[Async Producer] 平均延迟：{startTime.ElapsedMilliseconds * 1000.0 / messageCount:F2} μs/消息");

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        /// <summary>
        /// 带消息头的生产者 - 演示元数据传递
        /// Kafka 特性：消息头支持，传递上下文信息
        /// </summary>
        public void ProducerWithHeaders(string topic, int messageCount = 10)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"业务消息-{i}";
                    var key = $"biz-key-{i}";

                    var msg = new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    };

                    // 添加业务相关的头信息
                    msg.Headers.Add("trace-id", System.Text.Encoding.UTF8.GetBytes($"trace-{Guid.NewGuid():N}"));
                    msg.Headers.Add("user-id", System.Text.Encoding.UTF8.GetBytes($"user-{i % 10}"));
                    msg.Headers.Add("timestamp", System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                    msg.Headers.Add("source", System.Text.Encoding.UTF8.GetBytes("order-service"));
                    msg.Headers.Add("priority", new byte[] { (byte)(i % 3) }); // 优先级 0-2

                    var deliveryReport = producer.ProduceAsync(topic, msg).Result;
                    Console.WriteLine($"[Producer with Headers] 发送：{message}, 分区：{deliveryReport.Partition}, 头信息：trace-id, user-id, timestamp, source, priority");
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
