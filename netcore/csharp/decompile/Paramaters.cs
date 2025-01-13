using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace decompile
{
    public class Dog : Animal
    {

    }

    public class Cat : Animal
    {

    }

    public class Animal : IAnimal
    {

    }

    public interface IAnimal
    {
        void Feed()
        {
            Console.WriteLine("feed");
        }
    }
}
