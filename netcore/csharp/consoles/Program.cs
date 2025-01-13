using System.Reflection;
using consoles.Syntax;
using consoles.Threads;

namespace consoles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine(DateTime.Now.ToString("yyyyMMdd HHmmss"));
            // Console.ReadKey();

            // Console.WriteLine(Math.Floor(1.3));
            // Console.WriteLine(Math.Floor(1.5));
            // Console.WriteLine(Math.Floor(1.7));
            //
            // Console.WriteLine(Math.Floor(-1.3));
            // Console.WriteLine(Math.Floor(-1.5));
            // Console.WriteLine(Math.Floor(-1.7));

            // Console.WriteLine((int)(1.3));
            // Console.WriteLine((int)(1.5));
            // Console.WriteLine((int)(1.7));
            //
            // Console.WriteLine((int)(-1.3));
            // Console.WriteLine((int)(-1.5));
            // Console.WriteLine((int)(-1.7));


            // Console.WriteLine(CutOutDigital(0.2654,1));
            // Console.WriteLine(CutOutDigital(0.3654,1));
            // Console.WriteLine(CutOutDigital(0.4654,1));
            // Console.WriteLine(CutOutDigital(0.5654,1));
            // Console.WriteLine(CutOutDigital(0.6654,1));
            // Console.WriteLine(CutOutDigital(0.7654,1));
            // Console.WriteLine(CutOutDigital(0.8654,1));
            // Console.WriteLine(CutOutDigital(0.9654,1));
            // Console.WriteLine(CutOutDigital(1.9654e-15,1));

            // Run(typeof(DiagnosticBasicDemo));         
            Run(typeof(EqualsDemo));

        }


        public static double CutOutDigital(double input, int n)
        {
            //出现错误,科学基数法问题
            var strDecimal = input.ToString(".#################################");
            var index = strDecimal.IndexOf(".", StringComparison.Ordinal);
            if (index == -1 || strDecimal.Length < index + n + 1)
            {
                strDecimal = string.Format("{0:F" + n + "}", input);
            }
            else
            {
                int length = index;
                if (n != 0)
                {
                    length = index + n + 1;
                }
                strDecimal = strDecimal.Substring(0, length);
            }
            return double.Parse(strDecimal);
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