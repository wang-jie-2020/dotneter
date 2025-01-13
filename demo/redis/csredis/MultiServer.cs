using System;
using System.Reflection;

namespace Demo
{
    public class MultiServer
    {
        #region 公共调用方法

        public static void RunTest()
        {
            while (true)
            {
                try
                {
                    System.Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
                    string input = System.Console.ReadLine();
                    if (input == null)
                    {
                        continue;
                    }

                    if (input == "0")
                    {
                        break;
                    }

                    Type type = MethodBase.GetCurrentMethod().DeclaringType;
                    object o = Activator.CreateInstance(type);
                    type.InvokeMember("Method" + input, BindingFlags.Default | BindingFlags.InvokeMethod, null, o,
                        new object[] { });
                }
                catch (Exception e)
                {
                    throw;
                    //ExceptionHandler(e);
                }
            }
        }

        private static void ExceptionHandler(Exception e)
        {
            //ExceptionMessage emsg = new ExceptionMessage(e);
            //System.Console.WriteLine(emsg.ErrorDetails);
        }

        #endregion

        public MultiServer()
        {
        }

        public void Method1()
        {
            var csredis = new CSRedis.CSRedisClient(null,
                "127.0.0.1:6379,defaultDatabase=10,poolsize=10,ssl=false,writeBuffer=10240",
                "127.0.0.1:6479,defaultDatabase=11,poolsize=11,ssl=false,writeBuffer=10240",
                "127.0.0.1:6579,defaultDatabase=12,poolsize=12,ssl=false,writeBuffer=10240");

            RedisHelper.Initialization(csredis);

            for (int i = 0; i < 100000; i++)
            {
                RedisHelper.Set("key" + i, "123456", 100);
            }

            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine(RedisHelper.Get("key" + i));

            }
        }
    }
}
