using RabbitMQ.Client;
using System.Collections.Concurrent;
using System.Text;

namespace RabbitMQ_ErrorAppeare
{
    public class MQSender
    {
        private ConcurrentDictionary<string, RabbitMQInvoker> dic = new ConcurrentDictionary<string, RabbitMQInvoker>();

        public void Go()
        {
            var array = new[]
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7
            };

            while (true)
            {
                {
                    var index = new Random().Next(1, 7);
                    var invoker = dic.GetOrAdd(index.ToString(), (v) =>
                    {
                        return new RabbitMQInvoker(v);
                    });
                    invoker.Send(($"going -- {DateTime.Now.ToString("yyyyMMddHHmmss")}"));
                }

                {
                    var index = new Random().Next(1, 7);
                    var invoker = dic.GetOrAdd(index.ToString(), (v) =>
                    {
                        return new RabbitMQInvoker(v);
                    });
                    invoker.Send(($"going -- {DateTime.Now.ToString("yyyyMMddHHmmss")}"));
                }

                {
                    var index = new Random().Next(1, 7);
                    var invoker = dic.GetOrAdd(index.ToString(), (v) =>
                    {
                        return new RabbitMQInvoker(v);
                    });
                    invoker.Send(($"going -- {DateTime.Now.ToString("yyyyMMddHHmmss")}"));
                }

                Console.WriteLine($"going -- {DateTime.Now.ToString("yyyyMMddHHmmss")}");
                Thread.Sleep(5000);
            }
        }
    }
}
