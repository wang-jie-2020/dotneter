using System.Net;
using System.Reflection;
using Polly;

namespace Demo
{
    internal class RunSimpleDemo
    {
        public void Run()
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

                Type? type = MethodBase.GetCurrentMethod()?.DeclaringType;
                if (type != null)
                {
                    object? o = Activator.CreateInstance(type);
                    type.InvokeMember("Method" + input,
                        BindingFlags.Static | BindingFlags.Instance |
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.InvokeMethod,
                        null, o,
                        new object[] { });
                }
            }
        }

        void Method1()
        {
            Policy
                // 1. 指定要处理什么异常 或者 指定需要处理什么样的错误返回
                .Handle<HttpRequestException>().OrResult<HttpResponseMessage>(
                    r => r.StatusCode == HttpStatusCode.BadGateway)
                // 2. 指定重试次数和重试策略
                .Retry(3, (exception, retryCount, context) =>
                {
                    Console.WriteLine($"开始第 {retryCount} 次重试：");
                })
                // 3. 执行具体任务
                .Execute(ExecuteMockRequest);
        }

        HttpResponseMessage ExecuteMockRequest()
        {
            // 模拟网络请求
            Console.WriteLine("正在执行网络请求...");
            Thread.Sleep(3000);

            // 模拟网络错误
            return new HttpResponseMessage(HttpStatusCode.BadGateway);

        }
    }
}
