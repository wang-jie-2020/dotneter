using System.Reflection;
using basic.syntax;
using basic.诊断;
using Microsoft.Extensions.DependencyInjection;

namespace Feature
{
    internal class Program
    {
        static void Main(string[] args)
        {
            long seconds = 0l;

            Run(typeof(DiagnosticBasicDemo));
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

                    type.InvokeMember("Method" + input,
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.InvokeMethod,
                        null, o,
                        new object[] { });
                }
            }
        }


        public static string TimeTitcksToTime(long timeTitcks)
        {
            try
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(timeTitcks).ToLocalTime();
                return dt.ToString("yyyy/MM/dd HH:mm:ss");
            }
            catch
            {
                return new DateTime(timeTitcks).ToString("yyyy/MM/dd HH:mm:ss");
            }

            //return new DateTime(timeTitcks).ToString("yyyy/MM/dd HH:mm:ss");
        }

    }
}