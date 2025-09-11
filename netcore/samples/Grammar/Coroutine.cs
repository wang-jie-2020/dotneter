using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    internal class Coroutine
    {
        void Method1()
        {
            var numbers = ProduceEvenNumbers(5);
            Console.WriteLine("Caller: about to iterate.");
            foreach (int i in numbers)
            {
                Console.WriteLine($"Caller: {i}");
            }
        }

        IEnumerable<int> ProduceEvenNumbers(int upto)
        {
            Console.WriteLine("Iterator: start.");
            for (int i = 0; i <= upto; i += 2)
            {
                Console.WriteLine($"Iterator: about to yield {i}");
                yield return i;
                Console.WriteLine($"Iterator: yielded {i}");
            }
            Console.WriteLine("Iterator: end.");
        }
    }
}

