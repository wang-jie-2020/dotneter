using AspNetCore.Rabbitmq.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.Rabbitmq.Test
{
    public class RabbitMqIntegrationTest : BaseUnitTest
    {
        ITestOutputHelper Output;
        public RabbitMqIntegrationTest(ITestOutputHelper testOutputHelper)
        {
            Output = testOutputHelper;
        }

        /// <summary>
        /// ʹ�ö��з���
        /// </summary>
        [Fact]
        public void QueueTest()
        {
            int counter = 0;
            string queue = $"integration.{queue1}";

            //����
            var consumer = new RabbitConsumer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            consumer.Listen(queue, new ConsumeQueueOptions()
            {
                Arguments = arguments,
                AutoAck = false,
                AutoDelete = true,
                Durable = true,
                FetchCount = 1
            }, result =>
            {
                Output.WriteLine($"{queue}:" + result.Body);
                counter++;
                result.Commit();
            });

            //����
            var producer = new RabbitProducer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            producer.Publish(queue, "hello queue", new QueueOptions()
            {
                Arguments = arguments,
                AutoDelete = true,
                Durable = true
            });

            BlockUntil(() => counter >= 1, 3000);

            producer.Dispose();
            consumer.Dispose();

            Assert.Equal(1, counter);
        }
        /// <summary>
        /// ʹ��·�ɽ���������
        /// </summary>
        [Fact]
        public void DirectExchangeTest()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();//���յ���Ϣ����
            Dictionary<string, int> expectedDict = new Dictionary<string, int>();//���͵���Ϣ����
            string[] queues = new string[] { $"integration.{queue1}.direct", $"integration.{queue2}.direct" };
            var routeQueues = queues.Select(f => new RouteQueue()
            {
                Arguments = null,
                Queue = f,
                Route = f,
                Options = new QueueOptions()
                {
                    AutoDelete = true,
                    Durable = true,
                    Arguments = arguments
                }
            }).ToArray();

            //����
            var consumer = new RabbitConsumer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            foreach (var queue in queues)
            {
                dict[queue] = 0;
                expectedDict[queue] = 0;
                consumer.Listen(direct, queue, new ExchangeConsumeQueueOptions()
                {
                    Arguments = arguments,
                    AutoAck = false,
                    AutoDelete = true,
                    Durable = true,
                    FetchCount = 1,
                    RouteQueues = routeQueues,
                    Type = RabbitExchangeType.Direct
                }, result =>
                {
                    Output.WriteLine($"{queue}:" + result.Body);
                    dict[queue]++;
                    result.Commit();
                });
            }

            //����
            var producer = new RabbitProducer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            for (var i = 0; i < queues.Length; i++)
            {
                var queue = queues[i];
                expectedDict[queue]++;
                producer.Publish(direct, queue, "direct" + i, new ExchangeQueueOptions()
                {
                    Arguments = arguments,
                    AutoDelete = true,
                    Durable = true,
                    RouteQueues = routeQueues,
                    Type = RabbitExchangeType.Direct
                });
            }

            BlockUntil(() => dict.Sum(f => f.Value) >= queues.Length, 3000);

            producer.Dispose();
            consumer.Dispose();

            foreach (var queue in queues)
            {
                Output.WriteLine($"{queue}:{expectedDict[queue]}-{dict[queue]}");
                Assert.Equal(expectedDict[queue], dict[queue]);
            }
        }
        /// <summary>
        /// ʹ�÷������Ľ���������
        /// </summary>
        [Fact]
        public void FanoutExchangeTest()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();//���յ���Ϣ����
            string[] queues = new string[] { $"integration.{queue1}.fanout", $"integration.{queue2}.fanout" };
            var routeQueues = queues.Select(f => new RouteQueue()
            {
                Arguments = null,
                Queue = f,
                Route = string.Empty,
                Options = new QueueOptions()
                {
                    AutoDelete = true,
                    Durable = true,
                    Arguments = arguments
                }
            }).ToArray();

            //����
            var consumer = new RabbitConsumer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            foreach (var queue in queues)
            {
                dict[queue] = 0;
                consumer.Listen(fanout, queue, new ExchangeConsumeQueueOptions()
                {
                    Arguments = arguments,
                    AutoAck = false,
                    AutoDelete = true,
                    Durable = true,
                    FetchCount = 1,
                    RouteQueues = routeQueues,
                    Type = RabbitExchangeType.Fanout
                }, result =>
                {
                    Output.WriteLine($"{queue}:" + result.Body);
                    dict[queue]++;
                    result.Commit();
                });
            }

            //����
            var producer = new RabbitProducer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            for (var i = 0; i < queues.Length; i++)
            {
                producer.Publish(fanout, string.Empty, "fanout" + i, new ExchangeQueueOptions()
                {
                    Arguments = arguments,
                    AutoDelete = true,
                    Durable = true,
                    RouteQueues = routeQueues,
                    Type = RabbitExchangeType.Fanout
                });
            }

            BlockUntil(() => dict.Sum(f => f.Value) >= queues.Length * queues.Length, 3000);

            producer.Dispose();
            consumer.Dispose();

            foreach (var queue in queues)
            {
                Output.WriteLine($"{queue}:{dict[queue]}");
                Assert.Equal(queues.Length, dict[queue]);
            }
        }
        /// <summary>
        /// ʹ��Topic����������
        /// </summary>
        [Fact]
        public void TopicExchangeTest()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();//���յ���Ϣ����
            Dictionary<string, int> expectedDict = new Dictionary<string, int>();//���͵���Ϣ����
            string[] queues = new string[] { $"integration.{queue1}.topic", $"integration.{queue2}.topic" };
            var routeQueues = queues.Select(f => new RouteQueue()
            {
                Arguments = null,
                Queue = f,
                Route = $"#.{f}.#",
                Options = new QueueOptions()
                {
                    AutoDelete = true,
                    Durable = true,
                    Arguments = arguments
                }
            }).ToArray();

            //����
            var consumer = new RabbitConsumer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            foreach (var queue in queues)
            {
                dict[queue] = 0;
                expectedDict[queue] = 0;
                consumer.Listen(topic, queue, new ExchangeConsumeQueueOptions()
                {
                    Arguments = arguments,
                    AutoAck = false,
                    AutoDelete = true,
                    Durable = true,
                    FetchCount = 1,
                    RouteQueues = routeQueues,
                    Type = RabbitExchangeType.Topic
                }, result =>
                {
                    Output.WriteLine($"{queue}:" + result.Body);
                    dict[queue]++;
                    result.Commit();
                });
            }

            //����
            var producer = new RabbitProducer(hosts)
            {
                Password = password,
                Port = port,
                UserName = userName,
                VirtualHost = virtualHost
            };
            for (var i = 0; i < queues.Length; i++)
            {
                var queue = queues[i];
                expectedDict[queue]++;
                producer.Publish(topic, $"{i}.{queues[i]}.{i}", "topic" + i, new ExchangeQueueOptions()
                {
                    Arguments = arguments,
                    AutoDelete = true,
                    Durable = true,
                    RouteQueues = routeQueues,
                    Type = RabbitExchangeType.Topic
                });
            }

            BlockUntil(() => dict.Sum(f => f.Value) >= queues.Length, 3000);

            producer.Dispose();
            consumer.Dispose();

            foreach (var queue in queues)
            {
                Output.WriteLine($"{queue}:{expectedDict[queue]}-{dict[queue]}");
                Assert.Equal(expectedDict[queue], dict[queue]);
            }
        }
    }
}
