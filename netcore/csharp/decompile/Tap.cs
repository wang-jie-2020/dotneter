using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace decompile
{
    internal class Tap
    {
        public async Task<int> CalculateAsync()
        {
            int a = await Task.Run(() => 10);
            int b = await Task.Run(() => 20);
            return a + b;
        }
    }
}
