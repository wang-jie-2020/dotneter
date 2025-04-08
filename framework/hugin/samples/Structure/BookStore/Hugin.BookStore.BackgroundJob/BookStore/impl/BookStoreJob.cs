using System;
using System.Threading.Tasks;
using Hangfire;
using Hugin.BookStore.Btos;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Hugin.BookStore.impl
{
    public class BookStoreJob : BaseBackgroundJob, IBookStoreJob
    {
        private readonly ILogger<BookStoreJob> _logger;
        private readonly IRepository<Book, Guid> _bookRepository;

        public BookStoreJob(ILogger<BookStoreJob> logger,
            IRepository<Book, Guid> bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// 即时任务，类似LocalEventBus
        /// </summary>
        /// <returns></returns>
        public async Task ImmediatelyJobs()
        {
            await Task.Run(() =>
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
                BackgroundJob.Enqueue<BookStoreJob>(x => x.ExecImmediatelyJob1());
                BackgroundJob.Enqueue<BookStoreJob>(x => x.ExecImmediatelyJob2());
                BackgroundJob.Enqueue<BookStoreJob>(x => x.ExecImmediatelyJob3());
            });
        }

        [Queue("default")]
        public async Task ExecImmediatelyJob1()
        {
            Console.WriteLine("ExecImmediatelyJob1");
            await Task.CompletedTask;
        }

        [Queue("debug")]
        public async Task ExecImmediatelyJob2()
        {
            Console.WriteLine("ExecImmediatelyJob2");
            await Task.CompletedTask;
        }

        [Queue("neverfired")]
        public async Task ExecImmediatelyJob3()
        {
            Console.WriteLine("ExecImmediatelyJob3");
            await Task.CompletedTask;
        }

        /// <summary>
        /// 延迟任务
        /// </summary>
        /// <returns></returns>
        public async Task DelayedJobs()
        {
            await Task.Run(() =>
            {
                BackgroundJob.Schedule<BookStoreJob>(x => x.ExecDelayedJobs(),
                    TimeSpan.FromSeconds(10));
            });
        }

        public async Task ExecDelayedJobs()
        {
            Console.WriteLine("ExecDelayedJobs");
            await Task.CompletedTask;
        }

        /// <summary>
        /// 定时任务，相当于递归任务
        /// </summary>
        /// <returns></returns>
        public async Task RecurringJobs()
        {
            await Task.Run(() =>
            {
                RecurringJob.AddOrUpdate<BookStoreJob>(a => a.ExecRecurringJobs(),
                    Cron.Minutely(),
                    TimeZoneInfo.Local);
            });
        }

        public async Task ExecRecurringJobs()
        {
            Console.WriteLine("ExecRecurringJobs");
            await Task.CompletedTask;
        }

        /// <summary>
        /// 连续任务，不考虑
        /// </summary>
        /// <returns></returns>
        public async Task ContinuationJobs()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 演示Uow
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task UnitOfWorkJobs(BookJobArgs args)
        {
            await Task.Run(() =>
            {
                BackgroundJob.Enqueue<BookStoreJob>(x => x.ExecCheckBookCheckJob1(args));
                BackgroundJob.Enqueue<BookStoreJob>(x => x.ExecCheckBookCheckJob2(args));
            });
        }

        /*
         *  实现uow的方式1 - UnitOfWorkAttribute + public virtual 
         */
        [UnitOfWork]
        public virtual async Task ExecCheckBookCheckJob1(BookJobArgs args)
        {
            var bookId = args.BookId;
            var book = await _bookRepository.FindAsync(bookId);

            book.Price = new Random().Next() * 1111.11m;
            await _bookRepository.UpdateAsync(book);
            throw new Exception("Throw Uow Exception");
        }

        /*
         *  实现uow的方式2 - using (var uow = UnitOfWorkManager.Begin())
         *  更推荐，因为任务失败主动记录日志更好
         */
        public async Task ExecCheckBookCheckJob2(BookJobArgs args)
        {
            using (var uow = UnitOfWorkManager.Begin())
            {
                var bookId = args.BookId;
                var book = await _bookRepository.FindAsync(bookId);

                book.Price = new Random().Next() * 1111.11m;
                await _bookRepository.UpdateAsync(book);
                throw new Exception("Throw Uow Exception");
            }
        }
    }
}