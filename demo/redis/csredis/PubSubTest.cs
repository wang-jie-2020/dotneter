using System;
using System.Reflection;

namespace Demo
{
    /// <summary>
    /// 订阅、发布
    /// </summary>
    public class PubSubTest
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

        public PubSubTest()
        {
            //普通模式  var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,password=123,defaultDatabase=1,poolsize=50,ssl=false,writeBuffer=10240");
            var csredis =
                new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240");

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
        }

        public void Method1()
        {
            RedisHelper.Subscribe(("channel1", msg => Console.WriteLine(msg.Body)), ("channel2", msg => Console.WriteLine(msg.Body)));

            //模式订阅（通配符）
            RedisHelper.PSubscribe(new[] { "test*", "*test001", "test*002" }, msg =>
            {
                Console.WriteLine($"PSUB   {msg.MessageId}:{msg.Body}    {msg.Pattern}: chan:{msg.Channel}");
            });
            //模式订阅已经解决的难题：
            //1、分区的节点匹配规则，导致通配符最大可能匹配全部节点，所以全部节点都要订阅
            //2、本组 "test*", "*test001", "test*002" 订阅全部节点时，需要解决同一条消息不可执行多次

            //发布
            //RedisHelper.Publish("chan1", "123123123");
            //无论是分区或普通模式，rds.Publish 都可以正常通信
        }

    }
}
