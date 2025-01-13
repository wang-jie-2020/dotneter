using System;
using System.Reflection;

namespace Demo
{
    /// <summary>
    /// 管道，批量操作命令
    /// </summary>
    public class PipelineTest
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

        public PipelineTest()
        {
            //普通模式  var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,password=123,defaultDatabase=1,poolsize=50,ssl=false,writeBuffer=10240");
            var csredis =
                new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240");

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
        }

        public void Method1()
        {
            var ret1 = RedisHelper.StartPipe().Set("a", "1").Get("a").EndPipe();
            var ret2 = RedisHelper.StartPipe(p => p.Set("a", "1").Get("a"));

            var ret3 = RedisHelper.StartPipe().Get("b").Get("a").Get("a").EndPipe();

            //与 RedisHelper.MGet("b", "a", "a") 性能相比，经测试差之毫厘
        }
    }
}
