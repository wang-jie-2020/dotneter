using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Producer
{
    public class DefaultSender
    {
        private const string QueueName = "c_default";

        public void Send()
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

                    while (true)
                    {
                        Console.WriteLine("input message to send:");
                        var message = Console.ReadLine();

                        if (string.IsNullOrEmpty(message))
                        {
                            continue;
                        }

                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "", routingKey: QueueName, mandatory: false, basicProperties: null, body: body);
                    }
                }
            }
        }
    }
}
