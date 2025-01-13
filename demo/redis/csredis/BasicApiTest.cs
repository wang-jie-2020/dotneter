using System;
using System.Reflection;
using System.Threading;

namespace Demo
{
    /// <summary>
    /// 基本api操作
    /// </summary>
    public class BasicApiTest
    {
        #region 公共调用方法

        public static void RunTest()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
                    string input = Console.ReadLine();
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

        public BasicApiTest()
        {
            //普通模式  var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,password=123,defaultDatabase=1,poolsize=50,ssl=false,writeBuffer=10240");
            var csredis =
                new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240");

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

            RedisHelper.Set("time", DateTime.Now, 1); //设置过期时间
            Console.WriteLine(RedisHelper.Get<DateTime>("time"));

            Thread.Sleep(1100);
            Console.WriteLine(RedisHelper.Get<DateTime>("time"));
        }

        /// <summary>
        /// List
        /// </summary>
        public void Method2()
        {
            RedisHelper.RPush("list", "No1");
            RedisHelper.RPush("list", "No2");
            RedisHelper.LInsertBefore("list", "No2", "No2");

            Console.WriteLine($"list的长度为{RedisHelper.LLen("list")}");
            //Console.WriteLine($"list的长度为{RedisHelper.LLenAsync("list")}");//异步

            Console.WriteLine($"list的第二个元素为{RedisHelper.LIndex("list", 1)}");
            //Console.WriteLine($"list的第二个元素为{RedisHelper.LIndexAsync("list",1)}");//异步

        }

        /// <summary>
        /// Hash
        /// </summary>
        public void Method3()
        {
            RedisHelper.HSet("person", "name", "123");
            RedisHelper.HSet("person", "sex", "456");
            RedisHelper.HSet("person", "age", "789");
            RedisHelper.HSet("person", "adress", "10");

            Console.WriteLine($"person这个哈希中的age为{RedisHelper.HGet<int>("person", "age")}");
            //Console.WriteLine($"person这个哈希中的age为{RedisHelper.HGetAsync<int>("person", "age")}");//异步
        }

        /// <summary>
        /// Set
        /// </summary>
        public void Method4()
        {
            RedisHelper.SAdd("students", "zhangsan", "lisi");
            RedisHelper.SAdd("students", "wangwu");
            RedisHelper.SAdd("students", "zhaoliu");
            Console.WriteLine($"students这个集合的大小为{RedisHelper.SCard("students")}");
            Console.WriteLine($"students这个集合是否包含wagnwu:{RedisHelper.SIsMember("students", "wangwu")}");
        }
    }
}
