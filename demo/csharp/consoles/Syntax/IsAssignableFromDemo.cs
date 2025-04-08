using System.Reflection;

namespace consoles.Syntax
{
    internal class IsAssignableFromDemo
    {
        void Method1()
        {
            var isOk = false;

            //Console.WriteLine($" is {isOk}");

            /*
             *  typeof(基类).IsAssignableFrom(typeof(派生类)) = true
             */
            isOk = typeof(ADerived).IsAssignableFrom(typeof(A));
            Console.WriteLine($"typeof(ADerived).IsAssignableFrom(typeof(A)) == {isOk}");

            isOk = typeof(A).IsAssignableFrom(typeof(ADerived));
            Console.WriteLine($"typeof(A).IsAssignableFrom(typeof(ADerived)) == {isOk}");

            Console.WriteLine("typeof(基类).IsAssignableFrom(typeof(派生类)) = true");
            Console.WriteLine();

            /*
             *  typeof(接口).IsAssignableFrom(typeof(实现类及其派生类)) = true
             */
            isOk = typeof(IB).IsAssignableFrom(typeof(BImpl));
            Console.WriteLine($"typeof(IB).IsAssignableFrom(typeof(BImpl)) == {isOk}");
            isOk = typeof(IB).IsAssignableFrom(typeof(BDerived));
            Console.WriteLine($"typeof(IB).IsAssignableFrom(typeof(BDerived)) == {isOk}");

            Console.WriteLine("typeof(接口).IsAssignableFrom(typeof(实现类及其派生类)) = true");
            Console.WriteLine();

            /*
             *  简单概括，IsAssignableFrom如果前者是泛型的（即未指定具体类型时），一律false
             *  typeof(泛型).IsAssignableFrom(typeof(无论什么)) = false

             */
            isOk = typeof(IC<>).IsAssignableFrom(typeof(CImpl<>));
            Console.WriteLine($"typeof(IC<>).IsAssignableFrom(typeof(CImpl<>)) == {isOk}");
            isOk = typeof(IC<>).IsAssignableFrom(typeof(CImpl<string>));
            Console.WriteLine($"typeof(IC<>).IsAssignableFrom(typeof(CImpl<string>)) == {isOk}");

            isOk = typeof(CImpl<>).IsAssignableFrom(typeof(CImpl<string>));
            Console.WriteLine($"typeof(CImpl<>).IsAssignableFrom(typeof(CImpl<string>)) == {isOk}");
            isOk = typeof(CImpl<>).IsAssignableFrom(typeof(CDerived<>));
            Console.WriteLine($"typeof(CImpl<>).IsAssignableFrom(typeof(CDerived<>)) == {isOk}");
            isOk = typeof(CImpl<>).IsAssignableFrom(typeof(CDerived<string>));
            Console.WriteLine($"typeof(CImpl<>).IsAssignableFrom(typeof(CDerived<string>)) == {isOk}");

            Console.WriteLine("typeof(泛型).IsAssignableFrom(typeof(无论什么)) = false");
            Console.WriteLine();

            /*
             *  如果泛型接口或泛型类已经指定了具体类型，那么它可以以IsAssignableFrom做判断
             *  问题就来了，如何证明一个实现类是某一个泛型类或泛型接口的实现者，总不能构造无数泛型实例
             */
            isOk = typeof(IC<string>).IsAssignableFrom(typeof(CImpl<string>));
            Console.WriteLine($"typeof(IC<string>).IsAssignableFrom(typeof(CImpl<string>)) == {isOk}");
            isOk = typeof(CImpl<string>).IsAssignableFrom(typeof(CDerived<string>));
            Console.WriteLine($"typeof(CImpl<string>).IsAssignableFrom(typeof(CDerived<string>)) == {isOk}");

            Console.WriteLine("typeof(泛型带具体类型).IsAssignableFrom(typeof(实现或派生带具体类型)) = true");
            Console.WriteLine();

            /*
             *  typeof(泛型实现).GetGenericTypeDefinition() == typeof(泛型)   true;
             *
             *  typeof(泛型实现的派生).GetGenericTypeDefinition() == typeof(泛型)   false;
             *
             *  
             */
            isOk = typeof(CDerived<string>).IsGenericType;

            isOk = typeof(CDerived<string>).GetGenericTypeDefinition() == typeof(CDerived<>);
            Console.WriteLine($"typeof(CDerived<string>).GetGenericTypeDefinition() == typeof(CDerived<>) is {isOk}");

            isOk = typeof(CDerived<string>).GetGenericTypeDefinition() == typeof(CImpl<>);
            Console.WriteLine($"typeof(CDerived<string>).GetGenericTypeDefinition() == typeof(CImpl<>) is {isOk}");

            isOk = typeof(CDerived<string>).BaseType.GetGenericTypeDefinition() == typeof(CImpl<>);
            Console.WriteLine($"typeof(CDerived<string>).BaseType.GetGenericTypeDefinition() == typeof(CImpl<>) is {isOk}");

            var types1 = typeof(CDerived<string>).GetInterfaces();
            var types2 = typeof(CImpl<string>).GetInterfaces();
            isOk = typeof(CDerived<string>).GetInterfaces()[0].GetGenericTypeDefinition() == typeof(IC<>);
            Console.WriteLine($"typeof(CDerived<string>).GetInterfaces()[0].GetGenericTypeDefinition() == typeof(IC<>) is {isOk}");
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var givenTypeInfo = givenType.GetTypeInfo();

            if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenTypeInfo.GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenTypeInfo.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
        }

        public class A
        {
        }

        public class ADerived : A
        {
        }

        public interface IB
        {
        }

        public class BImpl : IB
        {
        }

        public class BDerived : BImpl
        {
        }

        public interface IC<T>
        {
        }

        public class CImpl<T> : IC<T>
        {
        }

        public class CDerived<T> : CImpl<T>
        {
        }
    }
}