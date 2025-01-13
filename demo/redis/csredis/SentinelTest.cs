using System;
using System.Reflection;

namespace Demo
{
    /// <summary>
    /// 基本api操作
    /// </summary>
    public class SentinelTest
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

        public SentinelTest()
        {
            //redismaster是哨兵配置中的服务器名词
            var csredis =
                new CSRedis.CSRedisClient("redismaster", new[] { "127.0.0.1:27000" });

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
        }

        /// <summary>
        /// string
        /// </summary>
        public void Method1()
        {
            RedisHelper.Set("name", "123");
            Console.WriteLine(RedisHelper.Get<string>("name"));
        }
    }
}
