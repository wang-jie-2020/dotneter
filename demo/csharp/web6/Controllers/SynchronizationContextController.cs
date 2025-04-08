using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace web6.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class SynchronizationContextController : ControllerBase
    {
        [HttpGet]
        public void Index()
        {
            Console.WriteLine($"当前线程是线程池线程?---{Thread.CurrentThread.IsThreadPoolThread}");

            /*
             *  在aspnetcore的web中不必考虑SynchronizationContext
             */
            
            {
                var context = System.Threading.SynchronizationContext.Current;
                Debug.Assert(context == null);
            }

            {
                Task.Run(() =>
                {
                    var context = System.Threading.SynchronizationContext.Current;
                    Debug.Assert(context == null);
                });
            }

            {
                Task.Factory.StartNew(() =>
                {
                    var context = System.Threading.SynchronizationContext.Current;
                    Debug.Assert(context == null);
                });
            }
        }
    }
}
