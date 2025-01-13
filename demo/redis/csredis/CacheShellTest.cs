using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Demo
{
    /// <summary>
    /// 缓存壳：对有缓存取缓存，无缓存取数据库的一个简化写法
    /// </summary>
    public class CacheShellTest
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

        public CacheShellTest()
        {
            //普通模式  var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,password=123,defaultDatabase=1,poolsize=50,ssl=false,writeBuffer=10240");
            var csredis =
                new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=0,poolsize=50,ssl=false,writeBuffer=10240");

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
        }

        public class Student
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }


        public void Method1()
        {
            List<Student> students = new List<Student>()
            {
                new Student() {Id = 1, Name = "1"},
                new Student() {Id = 2, Name = "2"},
                new Student() {Id = 3, Name = "3"},
                new Student() {Id = 4, Name = "4"},
            };

            Func<int, Student> search = (id) =>
            {
                Thread.Sleep(2000);
                return students.FirstOrDefault(o => o.Id == id);
            };

            ////不加缓存的时候，要从数据库查询
            //{
            //    Student s1 = search(1);
            //}


            ////一般的缓存代码，如不封装还挺繁琐的
            //{
            //    Student s2 = null;

            //    var cacheValue = RedisHelper.Get("test1");
            //    if (!string.IsNullOrEmpty(cacheValue))
            //    {
            //        s2 = JsonConvert.DeserializeObject<Student>(cacheValue);
            //    }
            //    else
            //    {
            //        s2 = search(1);
            //    }
            //    RedisHelper.Set("test1", JsonConvert.SerializeObject(s2), 10); //缓存10秒
            //}

            //使用缓存壳效果同上，以下示例使用 string 和 hash 缓存数据

            var t1 = RedisHelper.CacheShell("test1", 10, () => search(1));

            var t2 = RedisHelper.CacheShell("test2", "student:1", 10, () => search(1)); //Error:Hash时未能正确设置缓存时间

            var t3 = RedisHelper.CacheShell("test3", new[] { "stu1", "stu2" }, 10, notCacheFields => new[] {
                ("stu1", search(1)),
                ("stu2", search(2))
            });
        }
    }
}
