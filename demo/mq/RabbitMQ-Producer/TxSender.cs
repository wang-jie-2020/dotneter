using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ_Producer
{
    public class TxSender
    {
        private const string ExchangeName = "c_exchange_direct";
        private const string QueueName1 = "direct_1";
        private const string QueueName2 = "direct_2";

        private const string RoutingKey1 = "routingKey1";
        private const string RoutingKey2 = "routingKey2";

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
                        , type: "direct"
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
                        , routingKey: RoutingKey1
                        , arguments: null);

                    channel.QueueDeclare(
                        queue: QueueName2,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    channel.QueueBind(queue: QueueName2
                        , exchange: ExchangeName
                        , routingKey: RoutingKey2
                        , arguments: null);

                    //while (true)
                    //{
                    //Console.WriteLine("input message to send:");
                    var message = "input message to send:input message to send:input message to send:input message to send:input message to send:input message to send:input message to send:" + new Random().Next().ToString();  //Console.ReadLine();
                    Console.WriteLine(message);
                    //if (string.IsNullOrEmpty(message))
                    //{
                    //    continue;
                    //}

                    try
                    {
                        channel.TxSelect();
                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;

                        {
                            byte[] body = Encoding.UTF8.GetBytes($"{message} to {RoutingKey1}");
                            channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey1, mandatory: false, basicProperties: null, body: body);
                        }

                        {
                            byte[] body = Encoding.UTF8.GetBytes($"{message} to {RoutingKey2}");
                            channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey2, mandatory: false, basicProperties: null, body: body);
                        }

                        channel.TxCommit();
                    }
                    catch (Exception)
                    {
                        channel.TxRollback(); //事务回滚--前面的所有操作就全部作废了。。。。
                        throw;
                    }
                    //}
                }
            }
        }

        public void Send2()
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
                    //while (true)
                    //{
                    //Console.WriteLine("input message to send:");
                    var message = "input message to send:input message to send:input message to send:input message to send:input message to send:input message to send:input message to send:" + new Random().Next().ToString();  //Console.ReadLine();
                    Console.WriteLine(message);
                    //if (string.IsNullOrEmpty(message))
                    //{
                    //    continue;
                    //}

                    try
                    {
                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;

                        {
                            byte[] body = Encoding.UTF8.GetBytes($"{message} to {RoutingKey1}");
                            channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey1, mandatory: false, basicProperties: null, body: body);
                        }

                        {
                            byte[] body = Encoding.UTF8.GetBytes($"{message} to {RoutingKey2}");
                            channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey2, mandatory: false, basicProperties: null, body: body);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    //}
                }
            }
        }
    }
}
