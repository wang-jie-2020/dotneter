using Microsoft.AspNetCore.Mvc;

namespace Synchronization.Controllers
{
    [ApiController]
    public class ContextController : ControllerBase
    {
        [HttpGet]
        [Route("/context")]
        public void Index()
        {
            var thread = new Thread(() => { });
            var synchronizationContext = SynchronizationContext.Current;
            var executionContext = ExecutionContext.Capture();
            var taskScheduler = TaskScheduler.Default;

            Task.Run(() =>
            {
                var executionContext2 = ExecutionContext.Capture();
            });
        }

        [HttpGet]
        [Route("/start-tap-1")]
        public async Task StartTap1()
        {
            var thread1 = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"thread1:{thread1}");
            Console.WriteLine(SynchronizationContext.Current);
            
            Task.Run(async () =>
            {
                var thread2 = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"thread2:{thread2}");
                Console.WriteLine(SynchronizationContext.Current);

                // Thread.Sleep(1000);
                await Task.Delay(1000);

                var thread3 = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"thread3:{thread3}");
                Console.WriteLine(SynchronizationContext.Current);
            });

            var thread4 = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"thread4:{thread4}");
            Console.WriteLine(SynchronizationContext.Current);
        }

        [HttpGet]
        [Route("/start-tap-2")]
        public async Task StartTap2()
        {
            var thread1 = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"thread1:{thread1}");
            Console.WriteLine(SynchronizationContext.Current);
            
            await Task.Run(async () =>
            {
                var thread2 = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"thread2:{thread2}");
                Console.WriteLine(SynchronizationContext.Current);
                
                await Task.Delay(1000);

                var thread3 = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"thread3:{thread3}");
                Console.WriteLine(SynchronizationContext.Current);
            });

            var thread4 = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"thread4:{thread4}");
            Console.WriteLine(SynchronizationContext.Current);
        }
        
      
        [HttpGet]
        [Route("/start-tap-3")]
        public void StartTap3()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(SynchronizationContext.Current);
            var task = AsyncTask();
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(SynchronizationContext.Current);
            Console.WriteLine(task.Result);
            
            async Task<string> AsyncTask()
            {
                var task = Task.Run(() =>
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine(SynchronizationContext.Current);
                    Thread.Sleep(1000);
                    
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine(SynchronizationContext.Current);
                    return "123";
                });

                await task;
                
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(SynchronizationContext.Current);
                return task.Result;
            }
        }
    }
}