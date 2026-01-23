using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Grammar
{
    internal class Mem
    {

        unsafe void Method1()
        {
            int size = sizeof(IntPtr);
            Console.WriteLine(size);
        }

        unsafe void Method2()
        {
            int size = Marshal.SizeOf(typeof(double));
            Console.WriteLine(size);

            size = Marshal.SizeOf(typeof(Nullable<double>));
            Console.WriteLine(size);

            size = Marshal.SizeOf(typeof(Double));
            Console.WriteLine(size);
        }

        unsafe void Method3()
        {
            int size = sizeof(A);
            Console.WriteLine(size);

            size = Marshal.SizeOf(new A());
            Console.WriteLine(size);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    class A
    {
        public int? a;
    }
}
