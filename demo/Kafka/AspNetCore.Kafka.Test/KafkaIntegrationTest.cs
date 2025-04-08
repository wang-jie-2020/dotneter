using AspNetCore.Kafka.Integration;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.Kafka.Test
{
    public class KafkaIntegrationTest : BaseUnitTest
    {
        ITestOutputHelper Output;
        public KafkaIntegrationTest(ITestOutputHelper testOutputHelper)
        {
            Output = testOutputHelper;
        }

        /// <summary>
        /// ���ɲ���
        /// </summary>
        [Fact]
        public async Task IntegrationTest()
        {
            int counter = 0;

            //����
            var consumer = new KafkaConsumer($"integration.{group}", hosts)
            {
                EnableAutoCommit = false
            };
            await consumer.ListenAsync(new string[] { $"integration.{topic}" }, result =>
            {
                Output.WriteLine($"integration.{topic}({result.Key}):" + result.Message);
                counter++;
                result.Commit();
            });

            //����
            var producer = new KafkaProducer(hosts)
            {
                DefaultTopic = $"integration.{topic}",
                DefaultKey = $"integration.key"
            };
            await producer.PublishAsync("hello kafka");

            BlockUntil(() => counter >= 1, 3000);

            producer.Dispose();
            consumer.Dispose();

            Assert.Equal(1, counter);
        }
    }
}
