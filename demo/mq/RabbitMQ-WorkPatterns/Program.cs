using RabbitMQ_WorkPatterns;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("RabbitMQ 工作模式和确认机制演示");
        Console.WriteLine("===============================");

        var sender = new WorkPatternsSender();
        var consumer = new WorkPatternsConsumer();

        //// 演示工作队列模式
        //Console.WriteLine("\n1. 工作队列模式演示:");
        //Console.WriteLine("启动两个消费者...");

        //// 启动两个消费者
        //Task.Run(() => consumer.StartWorkQueueConsumer("Worker 1"));
        //Task.Run(() => consumer.StartWorkQueueConsumer("Worker 2"));
        //Thread.Sleep(1000);

        //// 发送消息
        //Console.WriteLine("发送 10 条消息到工作队列...");
        //sender.WorkQueueMode(10);

        //// 等待一段时间让消费者处理
        //Thread.Sleep(5000);

        // 演示发布/订阅模式
        //Console.WriteLine("\n2. 发布/订阅模式演示:");
        //Console.WriteLine("启动两个订阅者...");

        //Task.Run(() => consumer.StartPublishSubscribeConsumer("Subscriber 1"));
        //Task.Run(() => consumer.StartPublishSubscribeConsumer("Subscriber 2"));

        //Thread.Sleep(1000);

        //Console.WriteLine("发送 5 条广播消息...");
        //sender.PublishSubscribeMode(5);

        //Thread.Sleep(5000);

        // 演示路由模式
        //Console.WriteLine("\n3. 路由模式演示:");
        //Console.WriteLine("启动不同级别的消费者...");

        //Task.Run(() => consumer.StartRoutingConsumer("Info Consumer", "info"));
        //Task.Run(() => consumer.StartRoutingConsumer("Warning Consumer", "warning"));
        //Task.Run(() => consumer.StartRoutingConsumer("Error Consumer", "error"));
        //Task.Run(() => consumer.StartRoutingConsumer("All Consumer", "info", "warning", "error"));

        //Thread.Sleep(1000);

        //Console.WriteLine("发送不同级别的消息...");
        //sender.RoutingMode();

        //Thread.Sleep(3000);

        //// 演示主题模式
        //Console.WriteLine("\n4. 主题模式演示:");
        //Console.WriteLine("启动不同主题的消费者...");

        //Task.Run(() => consumer.StartTopicsConsumer("Orange Consumer", "*.orange.*"));
        //Task.Run(() => consumer.StartTopicsConsumer("Rabbit Consumer", "*.*.rabbit"));
        //Task.Run(() => consumer.StartTopicsConsumer("Lazy Consumer", "lazy.#"));

        //Console.WriteLine("发送不同主题的消息...");
        //sender.TopicsMode();

        //Thread.Sleep(3000);

        //// 演示确认模式
        //Console.WriteLine("\n5. 确认模式演示:");
        //Console.WriteLine("启动确认模式消费者...");

        //Task.Run(() => consumer.StartConfirmConsumer("Confirm Consumer"));

        //Console.WriteLine("发送带确认的消息...");
        //sender.ConfirmMode(500);

        //Thread.Sleep(3000);

        //// 演示异步确认模式
        Console.WriteLine("\n6. 异步确认模式演示:");
        Console.WriteLine("启动异步确认模式消费者...");

        Task.Run(() => consumer.StartAsyncConfirmConsumer("Async Confirm Consumer"));

        Console.WriteLine("发送带异步确认的消息...");
        sender.AsyncConfirmMode(500);

        Thread.Sleep(3000);

        //// 演示死信队列
        //Console.WriteLine("\n8. 死信队列演示:");
        //Console.WriteLine("启动正常队列消费者和死信队列消费者...");

        //Task.Run(() => consumer.StartNormalQueueConsumer("Normal Queue Consumer"));
        //Task.Run(() => consumer.StartDeadLetterQueueConsumer("Dead Letter Queue Consumer"));

        //Thread.Sleep(1000);

        //Console.WriteLine("发送可能被拒绝的消息...");
        //sender.DeadLetterQueueMode(5);

        //// 等待消息处理和进入死信队列
        //Thread.Sleep(10000);

        ////演示延迟队列
        //Console.WriteLine("\n7. 延迟队列演示:");
        //Console.WriteLine("启动延迟队列消费者...");

        //Task.Run(() => consumer.StartDelayQueueConsumer("Delay Queue Consumer"));

        //Thread.Sleep(1000);

        //Console.WriteLine("发送带延迟的消息...");
        //sender.DelayQueueMode(5);

        //// 等待延迟消息处理
        //Thread.Sleep(5000);

        Console.WriteLine("\n演示完成！按任意键退出...");
        Console.ReadKey();
    }

    static void Run(Type type)
    {
        while (true)
        {
            Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            if (input == "0")
            {
                break;
            }

            if (type != null)
            {
                object? o = Activator.CreateInstance(type);

                try
                {
                    type.InvokeMember("Method" + input,
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.InvokeMethod,
                        null, o,
                        new object[] { });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
