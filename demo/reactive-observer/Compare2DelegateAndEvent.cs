using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static observer_pattern.Compare2DelegateAndEvent;

namespace observer_pattern
{
    /*
     *  比较基础的生产消费模型可以通过委托或其派生实现,这类实现通常都会存在一些缺陷,比如delegate、event存在的内存泄露,没有明确的继续、错误、结束等等
     */
    internal class Compare2DelegateAndEvent
    {
        public void RunDemo()
        {
            while (true)
            {
                Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
                string input = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (input == "0")
                {
                    break;
                }

                Stopwatch sw = Stopwatch.StartNew();

                Type? type = MethodBase.GetCurrentMethod()?.DeclaringType;
                if (type != null)
                {
                    object? o = Activator.CreateInstance(type);
                    type.InvokeMember("Method" + input,
                        BindingFlags.Static | BindingFlags.Instance |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.InvokeMethod,
                        null, o,
                        new object[] { });
                }

                sw.Stop();
                Console.WriteLine("Method" + input + ":" + sw.ElapsedMilliseconds / 1000);
            }
        }


        public delegate void TrackLocationDelegate(Location location);

        internal class Producer
        {
            public TrackLocationDelegate trackLocationDelegate;

            public event TrackLocationDelegate TrackLocationEvent;

            public event EventHandler<LocationEventArgs> TrackLocationEventHandler;

            public Action<Location> TrackLocationAction;


            public void Produce()
            {
                foreach (var i in Enumerable.Range(10, 1))
                {
                    trackLocationDelegate?.Invoke(new Location(i, i));
                }

                foreach (var i in Enumerable.Range(20, 1))
                {
                    TrackLocationEvent?.Invoke(new Location(i, i));
                }

                foreach (var i in Enumerable.Range(30, 1))
                {
                    TrackLocationEventHandler?.Invoke(null, new LocationEventArgs(new Location(i, i)));
                }

                foreach (var i in Enumerable.Range(40, 1))
                {
                    TrackLocationAction?.Invoke(new Location(i, i));
                }
            }
        }

        internal class LocationEventArgs : EventArgs
        {
            public Location Location { get; set; }

            public LocationEventArgs(Location location)
            {
                this.Location = location;
            }
        }

        public void Method1()
        {
            var producer = new Producer();

            TrackLocationDelegate d1 = delegate (Location location) { Console.WriteLine(location.Latitude + "-" + location.Longitude); };

            producer.trackLocationDelegate = d1;
            producer.TrackLocationEvent += location => d1(location);
            producer.TrackLocationEventHandler += (sender, args) => d1(args.Location);
            producer.TrackLocationAction += location => d1(location);

            producer.Produce();
        }
    }
}
