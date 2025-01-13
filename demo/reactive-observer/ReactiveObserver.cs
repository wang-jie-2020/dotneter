using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace observer_pattern
{
    internal class ReactiveObserver
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

                //Stopwatch sw = Stopwatch.StartNew();

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

                //sw.Stop();
                //Console.WriteLine("Method" + input + ":" + sw.ElapsedMilliseconds / 1000);
            }
        }

        /*
         * ISubject 简化过程,它同时继承了 IObserver<T>, IObservable<T>，通过 AnonymousObserver 构造匿名的Observer
         *    
         */

        void Method1()
        {
            var subject = new Subject<string>();

            subject.Subscribe(new _4Method1()); //非匿名方式

            subject.OnNext("a");

            //watch 只会输出b
            var watch = subject.Subscribe(Console.WriteLine);  //匿名方式
            subject.OnNext("b");
            watch.Dispose();

            subject.OnNext("c");
        }

        class _4Method1 : IObserver<string>
        {

            public void OnCompleted()
            {
                Console.WriteLine("observer-completed");
            }


            public void OnError(Exception error)
            {
                Console.WriteLine("observer-error");
            }


            public void OnNext(string value)
            {
                Console.WriteLine("observer-" + value);
            }
        }

        void Method2()
        {
            Subject<string> messenger = new Subject<string>();
            messenger.Where(o => o.Length > 0).Subscribe(file => { Console.WriteLine("got file request: " + file); });

            var pathObservable = Observable.Return<string>("File 1");
            pathObservable.Subscribe(v => messenger.OnNext(v));

            var pathObservable2 = Observable.Return<string>("File 2");
            pathObservable2.Subscribe(v => messenger.OnNext(v));

            messenger.OnNext("File 3");
        }

        //线程操作,注意比较下一个
        void Method3()
        {
            Console.WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            var source = Observable.Create<int>(
                o =>
                {
                    Console.WriteLine("Invoked on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                    o.OnNext(1);
                    o.OnNext(2);
                    o.OnNext(3);
                    o.OnCompleted();
                    Console.WriteLine("Finished on threadId:{0}",
                        Thread.CurrentThread.ManagedThreadId);
                    return Disposable.Empty;
                });
            source
                //.SubscribeOn(Scheduler.ThreadPool)    //here
                .Subscribe(
                    o => Console.WriteLine("Received {1} on threadId:{0}",
                        Thread.CurrentThread.ManagedThreadId,
                        o),
                    () => Console.WriteLine("OnCompleted on threadId:{0}",
                        Thread.CurrentThread.ManagedThreadId));
            Console.WriteLine("Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        //线程操作,注意比较上一个
        void Method4()
        {
            Console.WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            var source = Observable.Create<int>(
                o =>
                {
                    Console.WriteLine("Invoked on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                    o.OnNext(1);
                    o.OnNext(2);
                    o.OnNext(3);
                    o.OnCompleted();
                    Console.WriteLine("Finished on threadId:{0}",
                        Thread.CurrentThread.ManagedThreadId);
                    return Disposable.Empty;
                });
            source
                .SubscribeOn(Scheduler.ThreadPool)
                .Subscribe(
                    o => Console.WriteLine("Received {1} on threadId:{0}",
                        Thread.CurrentThread.ManagedThreadId,
                        o),
                    () => Console.WriteLine("OnCompleted on threadId:{0}",
                        Thread.CurrentThread.ManagedThreadId));

            Console.WriteLine("Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        //From Action
        void Method5()
        {
            var start = Observable.Start(() =>
            {
                Console.Write("Working away");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(100);
                    Console.Write(".");
                }
            });
            start.Subscribe(
                unit => Console.WriteLine("Unit published"),
                () => Console.WriteLine("Action completed"));
        }

        //From Func
        void Method6()
        {
            var start = Observable.Start(() =>
            {
                Console.Write("Working away");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(100);
                    Console.Write(".");
                }
                return "Published value";
            });
            start.Subscribe(
                Console.WriteLine,
                () => Console.WriteLine("Action completed"));
        }

        //From Task
        void Method7()
        {
            var t = Task.Factory.StartNew(() => "Test");
            var source = t.ToObservable();
            source.Subscribe(
                Console.WriteLine,
                () => Console.WriteLine("completed"));
        }

        /*
         *  From Action、From Func、From Task 显得必要性不足，它们的实现效果似乎只是让发布者异步而不是持续异步提供结果集
         *
         *  综合当前已经看到的东西，在生产方法中持续的OnNext才是解决方式
         */

        void Method8()
        {
            var ob = Observable.Create<string>(
                observer =>
                {
                    var timer = new System.Timers.Timer();
                    timer.Interval = 1000;
                    timer.Elapsed += (s, e) => observer.OnNext("tick");
                    timer.Elapsed += (sender, e) => { Console.WriteLine(e.SignalTime); };
                    timer.Start();
                    return Disposable.Empty;
                });

            var subscription = ob.Subscribe(Console.WriteLine);
            Console.ReadLine();
            subscription.Dispose();
        }

        void Method9()
        {
            CancellationToken cancellationToken = default;

            var items = Observable.Create<int>(async (observer, ct) =>
            {
                using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, ct))
                {
                    foreach (var i in Enumerable.Range(1, 10))
                    {
                        await Task.Delay(1000, cancellationToken);
                        observer.OnNext(i);
                    }
                }
            });

            items.Subscribe(
                (i) =>
                {
                    Console.WriteLine($"value:{i}");
                },
                (error) =>
                {
                    Console.WriteLine("error");
                },
                () =>
                {
                    Console.WriteLine("completed");
                });
        }

        /*
         * 一个疑问点在于 如果生产者即时来时生产，但是没有消费者附加，会如何，有缓存？缺失部分或全部数据？
         *  看实现方式吧，如果有类似BlockingCollection的实现，则能够保证不缺失数据，测试了一下是有的，但这不能保证多线程下的一致性
         */

        void Method10()
        {

            Task.Run(async () =>
            {
                var items = Observable.Create<int>(async (observer, cancellationToken) =>
                {
                    foreach (var i in Enumerable.Range(1, 10))
                    {
                        await Task.Delay(1000, cancellationToken);
                        observer.OnNext(i);
                    }
                });

                await Task.Delay(10000);

                items.Subscribe(
                    (i) =>
                    {
                        Console.WriteLine($"value:{i}");
                    },
                    (error) =>
                    {
                        Console.WriteLine("error");
                    },
                    () =>
                    {
                        Console.WriteLine("completed");
                    });
            });
        }
    }
}
