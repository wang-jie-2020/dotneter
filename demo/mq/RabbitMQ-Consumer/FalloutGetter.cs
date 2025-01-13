using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ_Consumer
{
    public class FalloutGetter 
    {
        private const string ExchangeName = "c_exchange_fallout";
        private const string QueueName1 = "fallout_1";
        private const string QueueName2 = "fallout_2";
        private const string QueueName3 = "fallout_3";

        public void Get()
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "root",
                Password = "123456"
            };

            Task.Run(() =>
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            byte[] message = ea.Body;
                            Console.WriteLine($"{QueueName1}接收到信息为:" + Encoding.UTF8.GetString(message));
                        };

                        channel.BasicConsume(queue: QueueName1, autoAck: true, consumer: consumer);
                        Console.ReadKey();
                    }
                }
            });


            Task.Run(() =>
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            byte[] message = ea.Body;
                            Console.WriteLine($"{QueueName2}接收到信息为:" + Encoding.UTF8.GetString(message));
                        };

                        channel.BasicConsume(queue: QueueName2, autoAck: true, consumer: consumer);
                        Console.ReadKey();
                    }
                }
            });

            //ADD QUEUE IN CONSUMER WILL WORK
            Task.Run(() =>
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {

                        channel.QueueDeclare(
                            queue: QueueName3,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null
                        );

                        channel.QueueBind(queue: QueueName3
                            , exchange: ExchangeName
                            , routingKey: ""
                            , arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            byte[] message = ea.Body;
                            Console.WriteLine($"{QueueName3}接收到信息为:" + Encoding.UTF8.GetString(message));
                        };

                        channel.BasicConsume(queue: QueueName3, autoAck: true, consumer: consumer);
                        Console.ReadKey();
                    }
                }
            });
        }
    }
}
