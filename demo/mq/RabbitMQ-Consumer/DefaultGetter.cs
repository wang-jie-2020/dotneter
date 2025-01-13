using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ_Consumer
{
    public class DefaultGetter 
    {
        private const string QueueName = "c_default";

        public void Get()
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "root",
                Password = "123456"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );


                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                    };

                    channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
