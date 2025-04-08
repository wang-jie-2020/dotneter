using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Consul.Test
{
    public abstract class BaseUnitTest
    {
        protected static string address = "http://192.168.209.128:18401";
        protected static string datecenter = "dc1";
        protected static string token = "245d0a09";

        protected void BlockUntil(Func<bool> func, int milliseconds)
        {
            bool signaled = false;
            Task.WaitAny(Task.Delay(milliseconds), Task.Run(() =>
            {
                while (true && !signaled)
                {
                    if (func.Invoke())
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }));
            signaled = true;
        }
    }
}
