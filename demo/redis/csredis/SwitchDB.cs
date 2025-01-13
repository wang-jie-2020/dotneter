using System;
using System.Reflection;
using CSRedis;

namespace Demo
{
    /// <summary>
    /// 切换一个Server中的数据库
    /// </summary>
    public class SwitchDB
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

        public SwitchDB()
        {

        }

        public void Method1()
        {
            var connectionString = "127.0.0.1:6379,poolsize=50,ssl=false,writeBuffer=10240";

            var redis = new CSRedisClient[14];
            for (var a = 0; a < redis.Length; a++)
            {
                redis[a] = new CSRedisClient(connectionString + "; defualtDatabase=" + a);
                redis[a].Set("test" + a, "123");
            }

            //如果多个helper
            MyHelper1.Initialization(redis[1]);
            MyHelper2.Initialization(redis[2]);
        }
    }

    public abstract class MyHelper1 : RedisHelper<MyHelper1> { }

    public abstract class MyHelper2 : RedisHelper<MyHelper2> { }

}
