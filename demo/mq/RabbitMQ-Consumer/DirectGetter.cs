using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ_Consumer
{
    public class DirectGetter
    {
        private const string ExchangeName = "c_exchange_direct";
        private const string QueueName1 = "direct_1";
        private const string QueueName2 = "direct_2";
        private const string QueueName3 = "direct_1_1";

        private const string RoutingKey1 = "routingKey1";
        private const string RoutingKey2 = "routingKey2";

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

            //如果在已有的路由上 按同样的路由键再添加队列 对原队列不影响!新队列有值
            //Task.Run(() =>
            //{
            //    using (var connection = factory.CreateConnection())
            //    {
            //        using (var channel = connection.CreateModel())
            //        {
            //            channel.QueueDeclare(
            //                queue: QueueName3,
            //                durable: false,
            //                exclusive: false,
            //                autoDelete: false,
            //                arguments: null
            //            );

            //            channel.QueueBind(queue: QueueName3
            //                , exchange: ExchangeName
            //                , routingKey: RoutingKey1
            //                , arguments: null);

            //            var consumer = new EventingBasicConsumer(channel);
            //            consumer.Received += (model, ea) =>
            //            {
            //                byte[] message = ea.Body;
            //                Console.WriteLine($"{QueueName3}接收到信息为:" + Encoding.UTF8.GetString(message));
            //            };

            //            channel.BasicConsume(queue: QueueName3, autoAck: true, consumer: consumer);
            //            Console.ReadKey();
            //        }
            //    }
            //});

            //如果在已有的路由上 按同样的路由键再添加fanout交换
            Task.Run(() =>
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //创建新的fanout路由
                        channel.ExchangeDeclare(exchange: "extra-direct-fanout"
                            , type: "fanout"
                            , durable: false
                            , autoDelete: false
                            , arguments: null);

                        channel.ExchangeBind("extra-direct-fanout", ExchangeName, RoutingKey1);
                        channel.ExchangeBind("extra-direct-fanout", ExchangeName, RoutingKey2);
                        //channel.ExchangeBind("extra-direct-fanout", ExchangeName, "*");
                        //channel.ExchangeBind("extra-direct-fanout", ExchangeName, "*");

                        //新的fanout路由的队列
                        channel.QueueDeclare(
                            queue: "direct-fanout-1",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null
                        );

                        channel.QueueBind(queue: "direct-fanout-1"
                            , exchange: "extra-direct-fanout"
                            , routingKey: ""
                            , arguments: null);

                        channel.QueueDeclare(
                            queue: "direct-fanout-2",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null
                        );

                        channel.QueueBind(queue: "direct-fanout-2"
                            , exchange: "extra-direct-fanout"
                            , routingKey: ""
                            , arguments: null);


                        {
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                byte[] message = ea.Body;
                                Console.WriteLine($"direct-fanout-1接收到信息为:" + Encoding.UTF8.GetString(message));
                            };

                            channel.BasicConsume(queue: "direct-fanout-1", autoAck: true, consumer: consumer);
                        }

                        {
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                byte[] message = ea.Body;
                                Console.WriteLine($"direct-fanout-2接收到信息为:" + Encoding.UTF8.GetString(message));
                            };

                            channel.BasicConsume(queue: "direct-fanout-2", autoAck: true, consumer: consumer);
                        }

                        Console.ReadKey();
                    }
                }
            });
        }
    }
}
