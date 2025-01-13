using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace observer_pattern
{
    /*
     * C#中将生产消费模型抽象为IObservable、IObserver两个接口,这两个接口的示例如下,理解起来还是比较简单的
     *  System.Reactive在此基础上进行了派生,简化了调用过程
     *
     */
    internal class CSharpObserver
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

        void Method1()
        {
            LocationProvider provider = new LocationProvider();

            LocationObserver reporter1 = new LocationObserver("FixedGPS");
            reporter1.Subscribe(provider);

            LocationObserver reporter2 = new LocationObserver("MobileGPS");
            reporter2.Subscribe(provider);

            provider.TrackLocation(new Location(47.6456, -122.1312));
            reporter1.Unsubscribe();

            provider.TrackLocation(new Location(47.6677, -122.1199));
            provider.TrackLocation(null);

            provider.EndTransmission();
        }

        class LocationProvider : IObservable<Location>
        {
            private List<IObserver<Location>> observers;

            public LocationProvider()
            {
                observers = new List<IObserver<Location>>();
            }

            public IDisposable Subscribe(IObserver<Location> observer)
            {
                if (!observers.Contains(observer))
                    observers.Add(observer);

                return new Unsubscriber(observers, observer);
            }

            class Unsubscriber : IDisposable
            {
                private List<IObserver<Location>> _observers;
                private IObserver<Location> _observer;

                public Unsubscriber(List<IObserver<Location>> observers, IObserver<Location> observer)
                {
                    this._observers = observers;
                    this._observer = observer;
                }

                public void Dispose()
                {
                    if (_observers.Contains(_observer))
                        _observers.Remove(_observer);
                }
            }

            public void TrackLocation(Location? loc)
            {
                foreach (var observer in observers)
                {
                    if (!loc.HasValue)
                        observer.OnError(new LocationUnknownException());
                    else
                        observer.OnNext(loc.Value);
                }
            }

            public void EndTrack()
            {
                foreach (var observer in observers)
                {
                    observer.OnCompleted();
                }
            }

            public void EndTransmission()
            {
                foreach (var observer in observers.ToArray())
                    if (observers.Contains(observer))
                        observer.OnCompleted();

                observers.Clear();
            }
        }

        class LocationUnknownException : Exception
        {
            internal LocationUnknownException()
            { }
        }

        class LocationObserver : IObserver<Location>
        {
            private IDisposable unsubscriber;
            private string instName;

            public LocationObserver(string name)
            {
                this.instName = name;
            }

            public string Name
            { get { return this.instName; } }

            public virtual void Subscribe(IObservable<Location> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            public virtual void OnCompleted()
            {
                Console.WriteLine("The Location Tracker has completed transmitting data to {0}.", this.Name);
                this.Unsubscribe();
            }

            public virtual void OnError(Exception e)
            {
                Console.WriteLine("{0}: The location cannot be determined.", this.Name);
            }

            public virtual void OnNext(Location value)
            {
                Console.WriteLine("{2}: The current location is {0}, {1}", value.Latitude, value.Longitude, this.Name);
            }

            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }
        }

        //public class LocationTracker2 : IObservable<LocationTracker>
        //{
        //    public IDisposable Subscribe(IObserver<LocationTracker> observer)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public class LocationReporter2 : IObserver<LocationTracker2>
        //{
        //    public void OnCompleted()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void OnError(Exception error)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void OnNext(LocationTracker2 value)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
