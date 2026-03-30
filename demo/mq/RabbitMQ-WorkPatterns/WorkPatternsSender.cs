using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ_WorkPatterns
{
    public class WorkPatternsSender
    {
        private readonly ConnectionFactory _factory;

        public WorkPatternsSender()
        {
            _factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "root",
                Password = "123456",
                DispatchConsumersAsync = true,  //支持异步发送消息
            };
        }

        // 工作队列模式 - 多个消费者共同消费一个队列
        public void WorkQueueMode(int messageCount = 10)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string queueName = "work_queue";
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Work Queue Message {i}";
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true; // 消息持久化

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
                    Console.WriteLine($"[Work Queue] Sent: {message}");
                }
            }
        }

        // 发布/订阅模式 - 消息广播给多个消费者
        public void PublishSubscribeMode(int messageCount = 5)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string exchangeName = "fanout_exchange";
                channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Publish/Subscribe Message {i}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
                    Console.WriteLine($"[Publish/Subscribe] Sent: {message}");
                }
            }
        }

        // 路由模式 - 根据路由键发送消息
        public void RoutingMode()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string exchangeName = "direct_exchange";
                channel.ExchangeDeclare(exchange: exchangeName, type: "direct");

                var messages = new Dictionary<string, string>
                {
                    { "info", "Info: This is an information message" },
                    { "warning", "Warning: This is a warning message" },
                    { "error", "Error: This is an error message" }
                };

                foreach (var item in messages)
                {
                    var body = Encoding.UTF8.GetBytes(item.Value);
                    channel.BasicPublish(exchange: exchangeName, routingKey: item.Key, basicProperties: null, body: body);
                    Console.WriteLine($"[Routing] Sent to {item.Key}: {item.Value}");
                }
            }
        }

        // 主题模式 - 根据主题模式匹配发送消息
        public void TopicsMode()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string exchangeName = "topic_exchange";
                channel.ExchangeDeclare(exchange: exchangeName, type: "topic");

                var messages = new Dictionary<string, string>
                {
                    { "quick.orange.rabbit", "A quick orange rabbit" },
                    { "lazy.orange.elephant", "A lazy orange elephant" },
                    { "quick.orange.fox", "A quick orange fox" },
                    { "lazy.brown.fox", "A lazy brown fox" },
                    { "lazy.pink.rabbit", "A lazy pink rabbit" }
                };

                foreach (var item in messages)
                {
                    var body = Encoding.UTF8.GetBytes(item.Value);
                    channel.BasicPublish(exchange: exchangeName, routingKey: item.Key, basicProperties: null, body: body);
                    Console.WriteLine($"[Topics] Sent with routing key '{item.Key}': {item.Value}");
                }
            }
        }

        // 确认机制 - 确保消息被正确投递
        public void ConfirmMode(int messageCount = 5)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string queueName = "confirm_queue";
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                // 启用发布确认
                channel.ConfirmSelect();

                //// 注册确认回调
                //channel.BasicAcks += (sender, e) =>
                //{
                //    Console.WriteLine($"[Confirm] Message confirmed: DeliveryTag={e.DeliveryTag}");
                //};

                //channel.BasicNacks += (sender, e) =>
                //{
                //    Console.WriteLine($"[Confirm] Message rejected: DeliveryTag={e.DeliveryTag}, Multiple={e.Multiple}");
                //};

                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();
                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Confirm Mode Message {i}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                    Console.WriteLine($"[Confirm] Sent: {message}");

                    //等待单条消息确认
                    //bool confirmed = channel.WaitForConfirms();
                }

                // 等待所有消息被确认
                // bool allConfirmed = channel.WaitForConfirms();
                // Console.WriteLine($"[Confirm] All messages confirmed: {allConfirmed}");

                sw.Stop();
                Console.WriteLine($"ConfirmMode: " + sw.Elapsed);
            }
        }

        // 异步确认机制
        public void AsyncConfirmMode(int messageCount = 5)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string queueName = "async_confirm_queue";
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                // 启用发布确认
                // channel.ConfirmSelect();

                // 注册确认回调
                channel.BasicAcks += (sender, e) =>
                {
                    Console.WriteLine($"[Confirm] Message confirmed: DeliveryTag={e.DeliveryTag}");
                };

                channel.BasicNacks += (sender, e) =>
                {
                    Console.WriteLine($"[Confirm] Message rejected: DeliveryTag={e.DeliveryTag}, Multiple={e.Multiple}");
                };

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Async Confirm Message {i}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                    Console.WriteLine($"[Async Confirm] Sent: {message}");
                }
            }
        }

        // 延迟队列 - 消息在指定时间后才被消费
        public void DelayQueueMode(int messageCount = 5)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // 死信交换机
                string deadLetterExchange = "delay_dead_letter_exchange";
                // 实际消费队列
                string targetQueue = "delay_target_queue";
                // 延迟队列
                string delayQueue = "delay_queue";

                // 声明死信交换机
                channel.ExchangeDeclare(exchange: deadLetterExchange, type: "direct");

                // 声明实际消费队列，绑定到死信交换机
                channel.QueueDeclare(
                    queue: targetQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                channel.QueueBind(queue: targetQueue, exchange: deadLetterExchange, routingKey: "delay_routing_key");

                // 声明延迟队列，设置死信交换机和路由键
                var arguments = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", deadLetterExchange },
                    { "x-dead-letter-routing-key", "delay_routing_key" }
                };
                channel.QueueDeclare(
                    queue: delayQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: arguments
                );

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Delayed Message {i}";
                    var body = Encoding.UTF8.GetBytes(message);

                    // 设置消息的TTL（毫秒）
                    int delayTime = (i + 1) * 10000; // 1秒、2秒、3秒...
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.Expiration = delayTime.ToString();

                    channel.BasicPublish(exchange: "", routingKey: delayQueue, basicProperties: properties, body: body);
                    Console.WriteLine($"[Delay Queue] Sent: {message}, Delay: {delayTime/1000} seconds");
                }
            }
        }

        // 死信队列 - 处理失败的消息
        public void DeadLetterQueueMode(int messageCount = 5)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // 死信交换机
                string deadLetterExchange = "dlx_exchange";
                // 死信队列
                string deadLetterQueue = "dlx_queue";
                // 正常队列
                string normalQueue = "normal_queue";

                // 声明死信交换机
                channel.ExchangeDeclare(exchange: deadLetterExchange, type: "direct");

                // 声明死信队列
                channel.QueueDeclare(
                    queue: deadLetterQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );
                channel.QueueBind(queue: deadLetterQueue, exchange: deadLetterExchange, routingKey: "dlx_routing_key");

                // 声明正常队列，设置死信交换机和路由键
                var arguments = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", deadLetterExchange },
                    { "x-dead-letter-routing-key", "dlx_routing_key" },
                    { "x-message-ttl", 5000 } // 消息5秒后过期
                };
                channel.QueueDeclare(
                    queue: normalQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: arguments
                );

                for (int i = 0; i < messageCount; i++)
                {
                    var message = $"Message {i} - might be rejected";
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: normalQueue, basicProperties: properties, body: body);
                    Console.WriteLine($"[Dead Letter Queue] Sent: {message}");
                }
            }
        }
    }
}
