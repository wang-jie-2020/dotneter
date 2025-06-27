using System.Reflection;
using System.Text;

namespace Grammar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run(typeof(UriEn));
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
}