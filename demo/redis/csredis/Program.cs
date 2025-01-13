using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicApiTest.RunTest();
            //PubSubTest.RunTest();
            //CacheShellTest.RunTest();
            //PipelineTest.RunTest();
            //SwitchDB.RunTest();
            //MultiServer.RunTest();
            SentinelTest.RunTest();

            Console.ReadKey();
        }
    }
}