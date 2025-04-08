using System;
using System.Threading;
using AspNetCore.Zookeeper;

namespace AspNetCore.Locker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Zookeeper连接字符串，采用host:port格式，多个地址之间使用逗号（,）隔开
            string[] address = new[]
            {
                "vm.qq.com:2181",
                "vm.qq.com:2182",
                "vm.qq.com:2183"
            };

            //会话超时时间,单位毫秒
            int sessionTimeOut = 10000;

            //锁节点根路径
            string lockerPath = "/locker";

            for (var i = 0; i < 1; i++)
            {
                string client = "client" + i;

                //多线程模拟并发
                new Thread(() =>
                {
                    using (ZookeeperLocker zookeeperLocker = new ZookeeperLocker(lockerPath, sessionTimeOut, address))
                    {
                        string path = zookeeperLocker.CreateLock();
                        if (zookeeperLocker.Lock(path))
                        {
                            //模拟处理过程
                            Console.WriteLine($"【{client}】获得锁:{DateTime.Now}");
                            Thread.Sleep(30000);
                            Console.WriteLine($"【{client}】处理完成:{DateTime.Now}");
                        }
                        else
                        {
                            Console.WriteLine($"【{client}】获得锁失败:{DateTime.Now}");
                        }
                    }
                }).Start();
            }

            Console.ReadKey();
        }
    }
}
