using AspectCore.DynamicProxy;
using System;
using System.Reflection;
using System.Threading;

namespace hystrix.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var type = typeof(Program);

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
                    type.InvokeMember("Method" + input,
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.InvokeMethod,
                        null, o,
                        new object[] { });
                }
            }
        }

        static void InternalMethod1(string name)
        {
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                IPerson p = proxyGenerator.CreateClassProxy<Person>();
                p.HelloAsync(name ?? "default");
            }
        }

        static void Method1()
        {
            InternalMethod1(null);
        }

        static void InternalMethod2(string name)
        {
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                IPerson p = proxyGenerator.CreateClassProxy<Person>();
                var result = p.HiAsync(name ?? "default").Result;
                Console.WriteLine($"传入:{name}---结果:{result}");
            }
        }

        static void Method2()
        {
            InternalMethod2("旺财");
            InternalMethod2("来福");
        }
    }
}
