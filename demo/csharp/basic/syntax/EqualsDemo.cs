using System.Reflection;

namespace basic.syntax
{
    internal class EqualsDemo
    {
        void Method1()
        {
            var a = new SomeClass1 { Code = "A" };
            var b = new SomeClass1 { Code = "A" };

            Console.WriteLine(a == b); //引用比较似乎不能改写
            Console.WriteLine(a.Equals(b)); //可以改写，但是包含拆箱装箱，影响效率
        }

        class SomeClass1
        {
            public string Code { get; set; }

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;

                if (obj is SomeClass1 x)
                {
                    return Code.Equals(x.Code);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return Code.GetHashCode();
            }
        }

        void Method2()
        {
            var a = new SomeClass2 { Code = "A" };
            var b = new SomeClass2 { Code = "A" };

            Console.WriteLine(a == b); //引用必须要操作符重载
            Console.WriteLine(a.Equals(b)); //通过IEquatable更加效率
        }

        public class SomeClass2 : IEquatable<SomeClass2>
        {
            public string Code { get; set; }

            public bool Equals(SomeClass2 other)
            {
                return Code.Equals(other?.Code);
            }

            public static bool operator ==(SomeClass2 x, SomeClass2 y)
            {
                return x.Equals(y);
            }

            public static bool operator !=(SomeClass2 x, SomeClass2 y)
            {
                return !x.Equals(y);
            }
        }

        /*
         * 自定义类型的相等比较
         *      以运算符‘==’和‘！=’比较时，必须重载运算符，任何其他方式均不行
         *      以equals方法比较时：
         *          1.若实现了IEquatable，则直接走强类型比较方法，也省略了后一步的装箱拆箱
         *          2.重写object中的equals方法
         *
         *      以上的比较不会调用hash，也很好理解，不是集合中的也不至于到hash来获得唯一索引
         *
         *      如果是集合中需要判断相等，如distinct，则需要重写hash和equals，
         *          若实现了IEquatable，则直接走强类型比较方法，如未实现则走object的方法
         *      另一种做法是实现IEqualityComparer,作为distinct的comparer传递，但挺弱智的
         *
         */

        void Method3()
        {
            {
                var list1 = new List<SomeClass2>
                {
                    new SomeClass2 {Code = "A"},
                    new SomeClass2 {Code = "A"}
                };

                var list2 = list1.Distinct().ToList();
                Console.WriteLine(list2.Count);

                var list3 = list1.GroupBy(a => a);
                Console.WriteLine(list3.ToList().Count);
            }


            {
                var list1 = new List<SomeClass3>
                {
                    new SomeClass3 {Code = "A"},
                    new SomeClass3 {Code = "A"}
                };

                var list2 = list1.Distinct().ToList();
                Console.WriteLine(list2.Count);

                var list3 = list1.GroupBy(a => a);
                Console.WriteLine(list3.ToList().Count);
            }
        }

        public class SomeClass3 //: IEquatable<SomeClass3>  //若实现则走强类型的Equals(SomeClass3 other)
        {
            public string Code { get; set; }

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;

                if (obj is SomeClass3 x) return Code.Equals(x.Code);

                return false;
            }

            public bool Equals(SomeClass3 other)
            {
                return Code.Equals(other?.Code);
            }

            public override int GetHashCode()
            {
                return Code.GetHashCode();
            }
        }

        /*
         * 自定义类型的排序
         *  1.提供比较器 实现IComparer<SomeClass4>，作为参数
         *  2.提供比较方法 IComparable<SomeClass5>，明显舒服一些
         */
        void Method4()
        {
            var list1 = new List<SomeClass4>
            {
                new SomeClass4 {Code = "B"},
                new SomeClass4 {Code = "A"}
            };

            var list2 = list1.OrderBy(a => a, new SomeClass4Comparer()).ToList();
            Console.WriteLine(list2[0].Code);
            Console.WriteLine(list2[1].Code);
        }


        public class SomeClass4
        {
            public string Code { get; set; }
        }

        public class SomeClass4Comparer : IComparer<SomeClass4>
        {
            public int Compare(SomeClass4 x, SomeClass4 y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.Code, y.Code, StringComparison.Ordinal);
            }
        }

        void Method5()
        {
            var list1 = new List<SomeClass5>
            {
                new SomeClass5 {Code = "B"},
                new SomeClass5 {Code = "A"}
            };

            var list2 = list1.OrderBy(a => a).ToList();
            Console.WriteLine(list2[0].Code);
            Console.WriteLine(list2[1].Code);
        }


        public class SomeClass5 : IComparable<SomeClass5>
        {
            public string Code { get; set; }

            public int CompareTo(SomeClass5 other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return string.Compare(Code, other.Code, StringComparison.Ordinal);
            }
        }
    }
}
