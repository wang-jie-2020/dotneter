using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    /// <summary>
    ///     泛型类型参数的类型兼容性 引入了逆变in 协变out
    ///         
    /// 
    /// 
    /// 
    /// </summary>
    internal class GenericType
    {

        void GetN1(List<M> m)
        {

        }

        void GetN2<K>(List<K> m) where K : M
        {

        }

        void GetN3(IEnumerable<M> m)
        {

        }

        void Method1()
        {
            List<M> ms = new List<M>();
            List<M1> m1s = new List<M1>();

            GetN1(ms);
            //GetN1(m1s); //语法错误
            GetN1(m1s.Select(a => a as M).ToList());

            GetN2(ms);
            GetN2(m1s); //可行,隐藏了in,逆变

            GetN3(ms);
            GetN3(m1s); //可行,IEnumerable默认了in T,逆变
        }

        void Method2()
        {
            Action<M> mm = (_m) => { Console.WriteLine("mm"); };
            Action<M1> mm1 = (_m) => { Console.WriteLine("mm1"); };

            //mm = mm1;   //语法错误
            mm1 = mm;

            mm1(new M1());  //mm
        }

        void Method3()
        {
            Func<M> mm = () => { Console.WriteLine("mm");  return null; };
            Func<M1> mm1 = () => { Console.WriteLine("mm1"); return null; };

            mm = mm1;
            //mm1 = mm; // 语法错误
            mm();
        }

        //interface IA<K, V> where K : M where V : N
        //{
        //    V Get(K key);

        //    List<V> Get(List<K> keys);
        //}

        //class A<K, V> : IA<K, V> where K : M where V : N
        //{
        //    public V Get(K key)
        //    {
        //        return default(V);
        //    }

        //    public List<V> Get(List<K> keys)
        //    {
        //        return new List<V>();
        //    }
        //}

        class M
        {

        }

        class M1 : M
        {

        }

        //class N
        //{

        //}

        //class N1 : N
        //{

        //}
    }
}
