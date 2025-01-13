using System.Text;

namespace RabbitMQ_Producer
{
    public class FalloutSender
    {
        private const string ExchangeName = "c_exchange_fallout";
        private const string QueueName1 = "fallout_1";
        private const string QueueName2 = "fallout_2";

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
                    channel.ExchangeDeclare(exchange: ExchangeName
                        , type: "fanout"
                        , durable: false
                        , autoDelete: false
                        , arguments: null);

                    channel.QueueDeclare(
                        queue: QueueName1,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    channel.QueueBind(queue: QueueName1
                        , exchange: ExchangeName
                        , routingKey: ""
                        , arguments: null);

                    channel.QueueDeclare(
                        queue: QueueName2,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    //channel.QueueBind(queue: QueueName2
                    //    , exchange: ExchangeName
                    //    , routingKey: ""
                    //    , arguments: null);

                    //routingKey in fallout mode will be ignored
                    channel.QueueBind(queue: QueueName2
                        , exchange: ExchangeName
                        , routingKey: "routingKey"
                        , arguments: null);

                    while (true)
                    {
                        Console.WriteLine("input message to send:");
                        var message = Console.ReadLine();

                        if (string.IsNullOrEmpty(message))
                        {
                            continue;
                        }

                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //channel.BasicPublish(exchange: ExchangeName, routingKey: "", mandatory: false, basicProperties: null, body: body);
                        channel.BasicPublish(exchange: ExchangeName, routingKey: "routingKey", mandatory: false, basicProperties: null, body: body);
                    }
                }
            }
        }
    }
}
