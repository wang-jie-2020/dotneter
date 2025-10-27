using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    /// <summary>
    ///     泛型类型参数的类型兼容性 引入了逆变in 协变out
    /// </summary>
    internal class GenericType
    {
        /// <summary>
        ///     Dog -> Animal , List Dog -/> List Animal
        /// </summary>
        void Method1()
        {
            List<Animal> animals = new List<Animal>();
            List<Dog> dogs = new List<Dog>();

            Get1(animals);
            //Get1(dogs); //语法错误
            Get1(dogs.Select(a => a as Animal).ToList());

            Get2(animals);
            Get2(dogs); //可行,隐藏了in

            Get3(animals);
            Get3(dogs); //可行,IEnumerable默认了in T
            
            void Get1(List<Animal> _)
            {

            }

            void Get2<T>(List<T> _) where T : Animal
            {

            }

            void Get3(IEnumerable<Animal> _)
            {

            }
        }

        /// <summary>
        ///  使用(消费)时, 子参类型 -> 父参类型, 
        /// </summary>
        void Method2()
        {
            Action<Animal> animalAction = (_) => { Console.WriteLine("animal"); };
            Action<Dog> dogAction = (_) => { Console.WriteLine("dog"); };

            //animalAction = dogAction;   //语法错误
            dogAction = animalAction;

            dogAction(new Dog());   // animal
        }

        /// <summary>
        /// 返回(生产)时, 子参返回 -> 父参返回,
        /// </summary>
        void Method3()
        {
            Func<Animal> animalFunction = () => { Console.WriteLine("animal");  return null; };
            Func<Dog> dogFunction = () => { Console.WriteLine("dog"); return null; };

            animalFunction = dogFunction;
            //dogFunction = animalFunction; // 语法错误
            
            animalFunction();   // dog
        }
        
        class Animal
        {

        }

        class Dog : Animal
        {

        }
    }
}
