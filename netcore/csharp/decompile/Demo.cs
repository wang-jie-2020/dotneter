using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace decompile
{
    internal class Demo
    {
        public void Run()
        {
            var a = new Repository<IAnimal>();
            var b = new Repository<Animal>();
            var c = new Repository<Dog>();
            var d = new Repository<Cat>();
        }
    }
}
