using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace consoles.Syntax
{
    /// <summary>
    ///     内联函数,注意在release下运行测试,效果比较
    ///
    ///     这种提升存在一些限制,从网上的资料查到的:
    ///     (1)函数内部有循环语句、catch语句等复杂结构，都不做inline优化。
    ///     (2)函数体比较长的不做inline优化，只有比较简单的才可能inline优化。（有人说IL不足32字节才做inline）
    ///     (3)编译成机器码时，inline展开的代码比函数调用更短的，一定做inline。（注:如果参数多而代码少，就符合此情况）
    ///
    /// </summary>
    internal class MethodImplDemo
    {
        void Method1()
        {
            const int _max = 10000000;
            int sum = 0;

            Stopwatch s1 = Stopwatch.StartNew();
            for (int i = 0; i < _max; i++)
            {
                sum += Method1N();
            }
            s1.Stop();

            Stopwatch s2 = Stopwatch.StartNew();
            for (int i = 0; i < _max; i++)
            {
                sum += Method1E();
            }
            s2.Stop();

            Console.WriteLine(((double)(s1.Elapsed.TotalMilliseconds * 1000000) / _max).ToString("0.00 ns"));
            Console.WriteLine(((double)(s2.Elapsed.TotalMilliseconds * 1000000) / _max).ToString("0.00 ns"));
        }

        int Method1N()
        {
            return "one".Length + "two".Length + "three".Length +
                   "four".Length + "five".Length + "six".Length +
                   "seven".Length + "eight".Length + "nine".Length +
                   "ten".Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int Method1E()
        {
            return "one".Length + "two".Length + "three".Length +
                   "four".Length + "five".Length + "six".Length +
                   "seven".Length + "eight".Length + "nine".Length +
                   "ten".Length;
        }


    }
}
