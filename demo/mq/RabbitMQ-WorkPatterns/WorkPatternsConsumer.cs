using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ_WorkPatterns
{
    public class WorkPatternsConsumer
    {
        private readonly ConnectionFactory _factory;

        public WorkPatternsConsumer()
        {
            _factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "root",
                Password = "123456"
            };
        }

        // 工作队列模式消费者
        public void StartWorkQueueConsumer(string consumerName)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "work_queue";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            // 公平分发 - 一次只处理一条消息
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{consumerName}] Received: {message}");

                // 模拟处理时间
                Thread.Sleep(new Random().Next(100, 1000));

                // 手动确认消息
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                // Console.WriteLine($"[{consumerName}] Processed: {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"[{consumerName}] Started, waiting for messages...");
        }

        // 发布/订阅模式消费者
        public void StartPublishSubscribeConsumer(string consumerName)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            string exchangeName = "fanout_exchange";
            channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");

            // 创建临时队列
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{consumerName}] Received: {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($"[{consumerName}] Started, waiting for messages...");
        }

        // 路由模式消费者
        public void StartRoutingConsumer(string consumerName, params string[] routingKeys)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            string exchangeName = "direct_exchange";
            channel.ExchangeDeclare(exchange: exchangeName, type: "direct");

            var queueName = channel.QueueDeclare().QueueName;
            foreach (var routingKey in routingKeys)
            {
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{consumerName}] Received with routing key '{ea.RoutingKey}': {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($"[{consumerName}] Started with routing keys [{string.Join(", ", routingKeys)}], waiting for messages...");
        }

        // 主题模式消费者
        public void StartTopicsConsumer(string consumerName, params string[] routingPatterns)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            string exchangeName = "topic_exchange";
            channel.ExchangeDeclare(exchange: exchangeName, type: "topic");

            var queueName = channel.QueueDeclare().QueueName;
            foreach (var pattern in routingPatterns)
            {
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: pattern);
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{consumerName}] Received with routing key '{ea.RoutingKey}': {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($"[{consumerName}] Started with patterns [{string.Join(", ", routingPatterns)}], waiting for messages...");
        }

        // 确认模式消费者
        public void StartConfirmConsumer(string consumerName)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "confirm_queue";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // Console.WriteLine($"[{consumerName}] Received: {message}");
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            // Console.WriteLine($"[{consumerName}] Started, waiting for confirm messages...");
        }

        // 异步确认模式消费者
        public void StartAsyncConfirmConsumer(string consumerName)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "async_confirm_queue";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{consumerName}] Received: {message}");
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"[{consumerName}] Started, waiting for async confirm messages...");
        }

        // 延迟队列消费者 - 消费延迟后的消息
        public void StartDelayQueueConsumer(string consumerName)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            // 死信交换机
            string deadLetterExchange = "delay_dead_letter_exchange";
            // 实际消费队列
            string targetQueue = "delay_target_queue";

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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //Console.WriteLine($"[{consumerName}] Received delayed message: {message} at {DateTime.Now}");
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: targetQueue, autoAck: false, consumer: consumer);
            //Console.WriteLine($"[{consumerName}] Started, waiting for delayed messages...");
        }

        // 死信队列消费者 - 消费被拒绝或过期的消息
        public void StartDeadLetterQueueConsumer(string consumerName)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            // 死信交换机
            string deadLetterExchange = "dlx_exchange";
            // 死信队列
            string deadLetterQueue = "dlx_queue";

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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{consumerName}] Received dead letter message: {message}");
                // 可以在这里进行错误处理、重试或记录
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: deadLetterQueue, autoAck: false, consumer: consumer);
            Console.WriteLine($"[{consumerName}] Started, waiting for dead letter messages...");
        }

        // 正常队列消费者 - 模拟拒绝消息，使其进入死信队列
        public void StartNormalQueueConsumer(string consumerName)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            // 死信交换机
            string deadLetterExchange = "dlx_exchange";
            // 正常队列
            string normalQueue = "normal_queue";

            // 声明死信交换机
            channel.ExchangeDeclare(exchange: deadLetterExchange, type: "direct");

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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{consumerName}] Received: {message}");

                // 模拟处理失败，拒绝消息，使其进入死信队列
                Console.WriteLine($"[{consumerName}] Rejecting message: {message}");
                channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);
            };

            channel.BasicConsume(queue: normalQueue, autoAck: false, consumer: consumer);
            Console.WriteLine($"[{consumerName}] Started, will reject messages...");
        }
    }
}
