using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace web6.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class ExecutionContextController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ExecutionContextController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public void Index()
        {
            var sc1 = TaskScheduler.Default;
            var sc2 = TaskScheduler.Current;
            Debug.Assert(sc1 == sc2);
            Console.WriteLine($"typeof(TaskScheduler) == {sc2.GetType().Name}");

            var ec = ExecutionContext.Capture();

            //这里的代码会将 ec 的状态视为 ambient
            ExecutionContext.Run(ec, s =>
            {
            }, null);
        }

        [HttpGet]
        public void Index2()
        {
            var ec = ExecutionContext.Capture();

            new Thread(() =>
            {
                Thread.Sleep(1000);
                var ec1 = Thread.CurrentThread.ExecutionContext;
                Console.WriteLine($"thread ec1 == ec ? {ec == ec1}"); //true
            }).Start();

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                var ec1 = ExecutionContext.Capture();
                Console.WriteLine($"task ec1 == ec ? {ec == ec1}"); //true
            });
        }

        [HttpGet]
        public void Index3()
        {
            _threadStatic = "ThreadStatic保存的数据";
            _threadLocal.Value = "ThreadLocal保存的数据";
            _asyncLocal.Value = "AsyncLocal保存的数据";
            PrintValuesInAnotherThread();
        }

        [ThreadStatic]
        private static string _threadStatic;

        private static ThreadLocal<string> _threadLocal = new ThreadLocal<string>();
        private static AsyncLocal<string> _asyncLocal = new AsyncLocal<string>();

        private static void PrintValuesInAnotherThread()
        {
            Task.Run(() =>
            {
                Console.WriteLine($"ThreadStatic: {_threadStatic}");
                Console.WriteLine($"ThreadLocal: {_threadLocal.Value}");
                Console.WriteLine($"AsyncLocal: {_asyncLocal.Value}");
            });
        }

        [HttpGet]
        public void SubTask()
        {
            GetTask();
        }

        [HttpGet]
        public void GCollect()
        {
            GC.Collect();
        }

        private Task GetTask()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine($"---{DateTime.Now}---");
                    var a = ExecutionContext.Capture();
                    Console.WriteLine($"{_serviceProvider == null}");

                    Thread.Sleep(5000);
                }
            });
        }
    }
}