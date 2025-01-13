using System;
using System.Threading.Tasks;

namespace hystrix.Test
{
    public class Person : IPerson
    {
        [HystrixCommand(nameof(HelloFallBack1Async), MaxRetryTimes = 3, EnableCircuitBreaker = true)]
        public virtual async Task<string> HelloAsync(string name)
        {
            Console.WriteLine($"尝试执行{nameof(HelloAsync)}---" + name);

            string s = null;
            s.ToString();

            return await Task.FromResult("");
        }

        [HystrixCommand(nameof(HelloFallBack2Async))]
        public virtual async Task<string> HelloFallBack1Async(string name)
        {
            Console.WriteLine($"降级1---" + name);

            string s = null;
            s.ToString();

            return await Task.FromResult("");
        }

        public virtual async Task<string> HelloFallBack2Async(string name)
        {
            Console.WriteLine($"降级2---" + name);
            return await Task.FromResult($"success---{name}");
        }

        [HystrixCommand(nameof(HiFallBackAsync))]
        public async Task<string> HiAsync(string name)
        {
            Console.WriteLine($"尝试执行{nameof(HiAsync)}---" + name);

            string s = null;
            s.ToString();

            return await Task.FromResult("");
        }

        public virtual async Task<string> HiFallBackAsync(string name)
        {
            Console.WriteLine($"降级---" + name);
            return await Task.FromResult($"success---{name}");
        }


        //[HystrixCommand(nameof(AddFall))]
        //public virtual int Add(int i, int j)
        //{
        //    String s = null;
        //    //s.ToString();
        //    return i + j;
        //}
        //public int AddFall(int i, int j)
        //{
        //    return 0;
        //}

        //[HystrixCommand(nameof(TestFallBack))]
        //public virtual void Test(int i)
        //{
        //    Console.WriteLine("Test" + i);
        //}

        //public virtual void TestFallBack(int i)
        //{
        //    Console.WriteLine("Test" + i);
        //}
    }
}