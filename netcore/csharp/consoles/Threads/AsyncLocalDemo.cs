namespace consoles.Threads
{
    internal class AsyncLocalDemo
    {
        void Method1()
        {
            //临界资源
            string value = string.Empty;

            var task1 = Task.Run(async () =>
            {
                value = "a";

                Console.WriteLine($"task1-1 Before is {value}");
                await Task.Delay(5000);
                Console.WriteLine($"task1-2 After is {value}");
            });

            var task2 = Task.Run(async () =>
            {
                value = "b";

                Console.WriteLine($"task2-1 Before is {value}");
                await Task.Delay(1000);
                Console.WriteLine($"task2-2 After is {value}");

                value = "c";

                Console.WriteLine($"task2-3 Before is {value}");
                await Task.Delay(1000);
                Console.WriteLine($"task2-4 After is {value}");
            });

            task1.Wait();
            task2.Wait();
        }

        void Method2()
        {
            //临界资源
            AsyncLocal<string> value = new AsyncLocal<string>();

            var task1 = Task.Run(async () =>
            {
                value.Value = "a";

                Console.WriteLine($"task1-1 Before is {value.Value}");
                await Task.Delay(5000);
                Console.WriteLine($"task1-2 After is {value.Value}");
            });

            var task2 = Task.Run(async () =>
            {
                value.Value = "b";

                Console.WriteLine($"task2-1 Before is {value.Value}");
                await Task.Delay(1000);
                Console.WriteLine($"task2-2 After is {value.Value}");

                value.Value = "c";

                Console.WriteLine($"task2-3 Before is {value.Value}");
                await Task.Delay(1000);
                Console.WriteLine($"task2-4 After is {value.Value}");
            });

            task1.Wait();
            task2.Wait();
        }

        //protected class UnitOfWorkAccessor
        //{
        //    private readonly AsyncLocal<UnitOfWork> _current;

        //    public UnitOfWorkAccessor()
        //    {
        //        _current = new AsyncLocal<UnitOfWork>();
        //    }

        //    public void SetUnitOfWork(UnitOfWork uow)
        //    {
        //        _current.Value = uow;
        //    }

        //    public UnitOfWork? Uow => _current.Value;
        //}

        //protected class UnitOfWork : IDisposable
        //{
        //    public string Id { get; }

        //    public UnitOfWork(string id)
        //    {
        //        Id = id;
        //    }
        //    public UnitOfWork Outer { get; private set; }

        //    public void SetOuter(UnitOfWork outer)
        //    {
        //        Outer = outer;
        //    }

        //    public void Dispose()
        //    {
        //        Disposed(null, null);
        //    }

        //    public event EventHandler<EventArgs> Disposed;
        //}
    }
}
