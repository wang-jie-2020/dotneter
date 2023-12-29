using AspNetCore.Activemq.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.Activemq.Test
{
    public class ActiveMqIntegrationTest : BaseUnitTest
    {
        ITestOutputHelper Output;
        public ActiveMqIntegrationTest(ITestOutputHelper testOutputHelper)
        {
            Output = testOutputHelper;
        }


        /// <summary>
        /// ���м��ɲ���
        /// </summary>
        [Fact]
        public async Task QueueTest()
        {
            int counter = 0;
            var destination = $"integration.{queue}";

            //����
            var consumer = new ActiveConsumer(true, brokerUris, protocol: protocol)
            {
                Password = password,
                UserName = userName
            };
            await consumer.ListenAsync(new ListenOptions()
            {
                AutoAcknowledge = false,
                Destination = destination,
                Durable = false,
                FromQueue = true,
                PrefetchCount = 10,
                RecoverWhenNotAcknowledge = false
            }, result =>
            {
                Output.WriteLine($"{destination}:" + result.Message);
                counter++;
                result.Commit();
            });

            //����
            var producer = new ActiveProducer(true, brokerUris, protocol: protocol)
            {
                Password = password,
                UserName = userName
            };
            await producer.SendAsync(destination, "hello active");

            BlockUntil(() => counter >= 1, 3000);

            producer.Dispose();
            consumer.Dispose();

            Assert.Equal(1, counter);
        }
        /// <summary>
        /// Topic���ɲ���
        /// </summary>
        [Fact]
        public async Task TopicTest()
        {
            int counter = 0;
            var destination = $"integration.{topic}";

            //����
            var consumer = new ActiveConsumer(true, brokerUris, protocol: protocol)
            {
                Password = password,
                UserName = userName
            };
            await consumer.ListenAsync(new ListenOptions()
            {
                AutoAcknowledge = false,
                Destination = destination,
                Durable = false,
                FromQueue = false,
                PrefetchCount = 10,
                RecoverWhenNotAcknowledge = false
            }, result =>
            {
                Output.WriteLine($"{destination}:" + result.Message);
                counter++;
                result.Commit();
            });

            //����
            var producer = new ActiveProducer(true, brokerUris, protocol: protocol)
            {
                Password = password,
                UserName = userName
            };
            await producer.PublishAsync(destination, "hello active");

            BlockUntil(() => counter >= 1, 3000);

            producer.Dispose();
            consumer.Dispose();

            Assert.Equal(1, counter);
        }
        /// <summary>
        /// selector����
        /// </summary>
        [Fact]
        public async Task SelectorTest()
        {
            int counter = 0;
            var destination = $"integration.selector.{queue}";


            /*
             * ���ֱ��ʽ�� >,>=,<,<=,BETWEEN,=
             * �ַ����ʽ��=,<>,IN
             * IS NULL �� IS NOT NULL
             * �߼�AND, �߼�OR, �߼�NOT
             * 
             * ��������
             * ���֣�3.1415926�� 5
             * �ַ��� ��a����������е�����
             * NULL���ر�ĳ���
             * �������ͣ� TRUE��FALSE
             */

            //����
            var consumer = new ActiveConsumer(true, brokerUris, protocol: protocol)
            {
                Password = password,
                UserName = userName
            };
            await consumer.ListenAsync(new ListenOptions()
            {
                AutoAcknowledge = false,
                Destination = destination,
                Durable = false,
                FromQueue = true,
                PrefetchCount = 10,
                RecoverWhenNotAcknowledge = false,
                Selector = "apple = 10000 and xiaomi in ('1000','2000','3000') and huawei is null"
            }, result =>
            {
                Output.WriteLine($"{destination}:" + result.Message);
                counter++;
                result.Commit();
            });

            //����
            var producer = new ActiveProducer(true, brokerUris, protocol: protocol)
            {
                Password = password,
                UserName = userName
            };
            var dict = new Dictionary<string, object>()
            {
                { "apple", 10000 },
                { "xiaomi", "5000" }
            };
            await producer.SendAsync(new ActiveMessage()
            {
                Destination = destination,
                Message = "hello active",
                Properties = dict
            });

            dict["xiaomi"] = "2000";
            await producer.SendAsync(new ActiveMessage()
            {
                Destination = destination,
                Message = "hello active again",
                Properties = dict
            });

            BlockUntil(() => counter >= 1, 3000);

            producer.Dispose();
            consumer.Dispose();

            Assert.Equal(1, counter);
        }
    }
}
