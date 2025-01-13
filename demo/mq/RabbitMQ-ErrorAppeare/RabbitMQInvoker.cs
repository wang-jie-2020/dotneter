using Microsoft.AspNetCore.Razor.TagHelpers;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQ_ErrorAppeare
{
    public class RabbitMQInvoker
    {
        private static IModel receiveChannel = null;
        private static IModel sendChannel = null;
        private static IConnection sendConnection = null;
        private static IConnection receiveConnection = null;

        private const string ExchangeName = "c_exchange_direct";
        private const string QueueName1 = "direct_1";
        private const string QueueName2 = "direct_2";

        private const string RoutingKey1 = "routingKey1";
        private const string RoutingKey2 = "routingKey2";

        private string mark;

        public RabbitMQInvoker(string item)
        {
            mark = item;
        }

        public void Send(string message)
        {
            var flag = true;
            if (sendChannel == null || !sendChannel.IsOpen)
            {
                var factory = new RabbitMQ.Client.ConnectionFactory
                {
                    HostName = "127.0.0.1",
                    Port = 5672,
                    UserName = "root",
                    Password = "123456",
                    RequestedHeartbeat = 5
                };

                if (sendConnection != null)
                {
                    sendConnection.Abort();
                }
                sendConnection = factory.CreateConnection();
                sendChannel = sendConnection.CreateModel();//开辟新的信道通信
                sendChannel.ExchangeDeclare(exchange: ExchangeName
                                       , type: "direct"
                                       , durable: false
                                       , autoDelete: false
                , arguments: null);

                sendChannel.QueueDeclare(
                    queue: QueueName1,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                sendChannel.QueueBind(queue: QueueName1
                    , exchange: ExchangeName
                    , routingKey: RoutingKey1
                    , arguments: null);

                sendChannel.QueueDeclare(
                    queue: QueueName2,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                sendChannel.QueueBind(queue: QueueName2
                    , exchange: ExchangeName
                    , routingKey: RoutingKey2
                    , arguments: null);
            }
            try
            {
                IBasicProperties properties = sendChannel.CreateBasicProperties();
                properties.DeliveryMode = 1;
                properties.Expiration = "30000";

                var body = Encoding.UTF8.GetBytes(message);
                //sendChannel.BasicPublish(exchange: Config.Exchange,
                //                     routingKey: Config.RoutingKey,
                //                     basicProperties: properties,
                //                     body: body);

                sendChannel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey2, mandatory: false, basicProperties: null, body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
