using System.Collections.Concurrent;

namespace consoles.Threads
{
    internal class MultiThreadDemo
    {
        /*
         *  todo
         *  1.原子操作
         *  2.自旋锁---SpinLock    锁的内部 没有使用到 Win32 内核对象，所以只能进行线程之间的同步，不能进行跨进程同步。如果要完成跨进程的同步，需要使用 Monitor、Mutex 这样的方案。
         *  3.互斥锁---Mutex
         *  4.混合锁---Monitor...
         *  5.读写锁---ReaderWriterLock
         *  6.分布式锁---Zookeeper、Etcd
         *
         *  10.线程安全集合
         *  20.AutoResetEvent
         *  21.ManualResetEvent
         *
         */

        /*
         *  原子操作基于操作系统封装的api,可以简单理解其原理为对某块内存区域的读写控制
         */
        void Method1()
        {
            int i = 0;
            Interlocked.Increment(ref i);

            //int _lock = 0;
            //var _lock1 = Interlocked.CompareExchange(ref _lock, 1, 0);


            MultiThreadDemo _instance = null;
            Interlocked.CompareExchange(ref _instance, new MultiThreadDemo(), null);
        }

        /*
         * 自旋锁的实现基于原子操作Interlocked.CompareExchange
         *
         *  它使用一个数值来表示锁是否已经被获取，0表示未被获取，1表示已经获取，
         *      获取锁时会先使用原子操作设置数值为1，然后检查修改前的值是否为0，如果为0则代表获取成功，否则继续重试直到成功为止，释放锁时会设置数值为0，其他正在获取锁的线程会在下一次重试时成功获取，
         *      使用原子操作的原因是，它可以保证多个线程同时把数值0修改到1时，只有一个线程可以观察到修改前的值为0，其他线程观察到修改前的值为1
         *      这么表示：Interlocked.CompareExchange(ref _isLocked, 1, 0) == 0;
         *
         *
         * 使用自旋锁有个需要注意的问题，自旋锁保护的代码应该在非常短的时间内执行完毕，如果代码长时间运行则其他需要获取锁的线程会不断重试并占用逻辑核心，影响其他线程运行，
         * 此外，如果 CPU 只有一个逻辑核心，自旋锁在获取失败时应该立刻调用 Thread.Yield 函数提示操作系统切换到其他线程，因为一个逻辑核心同一时间只能运行一个线程，在切换线程之前其他线程没有机会运行，也就是切换线程之前自旋锁没有机会被释放
         */
        void Method2()
        {
            var count = 0;
            var taskList = new Task[10];
            SpinLock _spinLock = new SpinLock();
            for (int i = 0; i < taskList.Length; i++)
            {
                taskList[i] = Task.Run(() =>
                {
                    bool _lock = false;
                    for (int j = 0; j < 10_000_000; j++)
                    {
                        try
                        {
                            _spinLock.Enter(ref _lock);
                            count++;
                            _spinLock.Exit();
                            _lock = false;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        finally
                        {
                            if (_lock)
                            {
                                _spinLock.Exit();
                            }
                        }
                    }
                });
            }

            Task.WaitAll(taskList);
            Console.WriteLine($"结果:{count}");
        }

        void Method4()
        {
            try
            {
                var _syncRoot = new object();

                var locked = Monitor.TryEnter(_syncRoot, 5000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /*
         *  成功获取读锁的充要条件是没有任何写锁.
         *  成功获取写锁的充要条件是没有任何锁.
         *
         *
         *
         */
        void Method5()
        {
            ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

            // 如果某个线程进入了写入模式，那么其他线程无论是要写入还是读取，都是会被阻塞的
            //Task t_write1 = new Task(WriteSomething);
            //t_write1.Start();
            //Console.WriteLine("{0} Create Thread ID {1} , Start WriteSomething", DateTime.Now.ToString("hh:mm:ss fff"), t_write1.GetHashCode());

            //Task t_write2 = new Task(WriteSomething);
            //t_write2.Start();
            //Console.WriteLine("{0} Create Thread ID {1} , Start WriteSomething", DateTime.Now.ToString("hh:mm:ss fff"), t_write2.GetHashCode());

            //当某个线程进入读取模式时，此时其他线程依然能进入读取模式，假设此时一个线程要进入写入模式，那么他不得不被阻塞。直到读取模式退出为止。
            Task t_read1 = new Task(ReadSomething);
            t_read1.Start();
            Console.WriteLine("{0} Create Thread ID {1} , Start ReadSomething", DateTime.Now.ToString("hh:mm:ss fff"),
                t_read1.GetHashCode());

            //Task t_read2 = new Task(ReadSomething);
            //t_read2.Start();
            //Console.WriteLine("{0} Create Thread ID {1} , Start ReadSomething", DateTime.Now.ToString("hh:mm:ss fff"), t_read2.GetHashCode());
            //Console.ReadKey();

            void ReadSomething()
            {
                Console.WriteLine(
                    "*************************************读取模式*********************************************");
                Console.WriteLine("{0} Thread ID {1} Begin EnterReadLock...", DateTime.Now.ToString("hh:mm:ss fff"),
                    Task.CurrentId.GetHashCode());
                rwl.EnterReadLock(); //进入读取模式锁定状态
                try
                {
                    Console.WriteLine("{0} Thread ID {1} reading sth...", DateTime.Now.ToString("hh:mm:ss fff"),
                        Task.CurrentId.GetHashCode());
                    Thread.Sleep(5000); //模拟读取信息
                    Console.WriteLine("{0} Thread ID {1} reading end.", DateTime.Now.ToString("hh:mm:ss fff"),
                        Task.CurrentId.GetHashCode());
                }
                finally
                {
                    rwl.ExitReadLock();
                    Console.WriteLine("{0} Thread ID {1} ExitReadLock...", DateTime.Now.ToString("hh:mm:ss fff"),
                        Task.CurrentId.GetHashCode());
                }
            }

            void WriteSomething()
            {
                Console.WriteLine(
                    "***********************************写入模式***************************************************");
                Console.WriteLine("{0} Thread ID {1} Begin EnterWriteLock...", DateTime.Now.ToString("hh:mm:ss fff"),
                    Task.CurrentId.GetHashCode());
                rwl.EnterWriteLock(); //进入写入模式锁定状态
                try
                {
                    Console.WriteLine("{0} Thread ID {1} writing sth...", DateTime.Now.ToString("hh:mm:ss fff"),
                        Task.CurrentId.GetHashCode());
                    Thread.Sleep(10000); //模拟写入信息
                    Console.WriteLine("{0} Thread ID {1} writing end.", DateTime.Now.ToString("hh:mm:ss fff"),
                        Task.CurrentId.GetHashCode());
                }
                finally
                {
                    rwl.ExitWriteLock();
                    Console.WriteLine("{0} Thread ID {1} ExitWriteLock...", DateTime.Now.ToString("hh:mm:ss fff"),
                        Task.CurrentId.GetHashCode());
                }
            }
        }

        void Method10()
        {
            //IProducerConsumerCollection<T>的实现
            ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();
            ConcurrentStack<int> concurrentStack = new ConcurrentStack<int>();
            ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();
            BlockingCollection<int> blockingCollection = new BlockingCollection<int>();

            ConcurrentDictionary<int, int> concurrentDictionary = new ConcurrentDictionary<int, int>();

            concurrentQueue.Enqueue(1);
        }

        /*
            AutoResetEvent如果处于非信号状态，线程将阻止，直到调用AutoResetEvent.Set以释放等待的线程
            线程通过调用AutoResetEvent.WaitOne来等待信号,在释放单个等待线程之前保持信号，然后自动返回到非信号状态。
        
            如果没有线程在等待，状态将无限期保持信号。如果线程在处于信号状态时AutoResetEvent调用WaitOne，则线程不会阻止。立即AutoResetEvent释放线程并返回到非信号状态。
        
            注意在WaitOne得到信号通知之后 自动 返回到非信号状态
         */
        void Method20()
        {
            AutoResetEvent are = new AutoResetEvent(false);

            for (int i = 1; i < 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(250);

            for (int i = 1; i < 4; i++)
            {
                Console.WriteLine("Press Enter to release another thread.");
                Console.ReadLine();
                are.Set();
                Thread.Sleep(250);
            }

            void ThreadProc()
            {
                string? name = Thread.CurrentThread.Name;

                Console.WriteLine("{0} waits on AutoResetEvent #1.", name);
                are.WaitOne(Timeout.Infinite);
                Console.WriteLine("{0} is released from AutoResetEvent #1.", name);

                Console.WriteLine("{0} ends.", name);
            }
        }

        /*
            当线程开始必须在其他线程继续之前完成的活动时，它会调用 ManualResetEvent.Reset 以进入 ManualResetEvent 非信号状态。
            调用 ManualResetEvent.WaitOne 块的线程正在等待信号。 当控制线程完成活动时，它会调用 ManualResetEvent.Set 来指示等待的线程可以继续。 释放所有等待的线程。
            一旦发出信号，将保持信号状态， ManualResetEvent 直到通过调用 Reset() 方法手动重置。 也就是说，调用 立即 WaitOne 返回。
         
            注意在WaitOne得到信号通知之后 不会 返回到非信号状态
         */
        void Method21()
        {
            ManualResetEvent mre = new ManualResetEvent(false);

            for (int i = 0; i <= 2; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            mre.Set();
            Console.ReadLine();

            for (int i = 3; i <= 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            mre.Reset();

            Thread t5 = new Thread(ThreadProc);
            t5.Name = "Thread_5";
            t5.Start();

            Thread.Sleep(500);
            mre.Set();

            void ThreadProc()
            {
                string? name = Thread.CurrentThread.Name;

                Console.WriteLine(name + " starts and calls mre.WaitOne()");

                mre.WaitOne();

                Console.WriteLine(name + " ends.");
            }
        }

        /*
         *  ManualResetEventSlim 是性能更好的轻量替代,它不再基于EventWaitHandle实现
         */
        void Method22()
        {
            ManualResetEventSlim mre = new ManualResetEventSlim(false);

            for (int i = 0; i <= 2; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            mre.Set();
            Console.ReadLine();

            for (int i = 3; i <= 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            mre.Reset();

            Thread t5 = new Thread(ThreadProc);
            t5.Name = "Thread_5";
            t5.Start();

            Thread.Sleep(500);
            mre.Set();

            void ThreadProc()
            {
                string? name = Thread.CurrentThread.Name;

                Console.WriteLine(name + " starts and calls mre.WaitOne()");

                mre.Wait();

                Console.WriteLine(name + " ends.");
            }
        }

        /*
         *
         *  写在前:在网上的示例中用了static关键字,但测试下来似乎无关
         *  调试中只在RELEASE中报出了问题,查到的资料中描述volatile关键字的概念似乎有些绕:
         *      volatile 关键字只能应用于 class 或 struct 的字段。不能将局部变量声明为 volatile
         *      它有些类似于lock(引用类型)中的锁类型要求,想象一下lock string会出现的场景
         *  
         *  有些合理的解释时编译优化中传递值类型,在每个线程中访问时发生了复制同时缓存在上下文中,如果它只能标识在struct中那么就很合理了,但class类型也能加以标记,这和上面的解释还是冲突的
         *      
         *  使用建议:多线程时需要额外注意值类型的操作和锁定,通常的替代方案是InterLocked
         *      
         */

        int bookNum = 0; //bad
        volatile int bookNum2 = 0; //good
        string bookNames = string.Empty; //good

        void Method30()
        {
            Thread juster = new Thread(() =>
            {
                Console.WriteLine("juster没带书，等待家长送书到学校...");

                while (bookNum == 0)
                {
                }

                Console.WriteLine("juster拿到书，开始上课听讲。");
            });
            juster.Start();

            Thread parent = new Thread(() =>
            {
                Console.WriteLine("parent在屋里找书中...");

                Thread.Sleep(2000);

                Console.WriteLine("parent找到了书之后，送往学校...");

                SendBook();
            });
            parent.Start();

            void SendBook()
            {
                bookNum = 1;
            }
        }

        void Method31()
        {
            Thread juster = new Thread(() =>
            {
                Console.WriteLine("juster没带书，等待家长送书到学校...");

                while (bookNum2 == 0)
                {
                }

                Console.WriteLine("juster拿到书，开始上课听讲。");
            });
            juster.Start();

            Thread parent = new Thread(() =>
            {
                Console.WriteLine("parent在屋里找书中...");

                Thread.Sleep(2000);

                Console.WriteLine("parent找到了书之后，送往学校...");

                SendBook();
            });
            parent.Start();

            void SendBook()
            {
                bookNum2 = 1;
            }
        }

        void Method32()
        {
            Thread juster = new Thread(() =>
            {
                Console.WriteLine("juster没带书，等待家长送书到学校...");

                while (bookNames == string.Empty)
                {
                }

                Console.WriteLine("juster拿到书，开始上课听讲。");
            });
            juster.Start();

            Thread parent = new Thread(() =>
            {
                Console.WriteLine("parent在屋里找书中...");

                Thread.Sleep(2000);

                Console.WriteLine("parent找到了书之后，送往学校...");

                SendBook();
            });
            parent.Start();

            void SendBook()
            {
                bookNames = "1";
            }
        }

        void Method33()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(10000);
                    bookNum += 1;
                }
            });


            var list = new List<string>();
            var list2 = list.Where(p => p == "1").Select(p => p).ToList();
		


            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    Console.WriteLine($"bookNum = { bookNum }");   
                }
            });
        }
    }
}