using System;
using System.Threading.Tasks;
using Hugin.BookStore.Btos;
using Swashbuckle.AspNetCore.Annotations;

namespace Hugin.BookStore.impl
{
    [SwaggerTag("Hangfire Jobs")]
    public class BackgroundJobAppService : BaseAppService
    {
        private readonly IBookStoreJob _bookStoreJob;

        public BackgroundJobAppService(IBookStoreJob bookStoreJob)
        {
            _bookStoreJob = bookStoreJob;
        }

        public async Task ImmediatelyJobs()
        {
            await _bookStoreJob.ImmediatelyJobs();
        }

        public async Task DelayedJobs()
        {
            await _bookStoreJob.DelayedJobs();
        }

        public async Task RecurringJobs()
        {
            await _bookStoreJob.RecurringJobs();
        }

        public async Task ContinuationJobs()
        {
            await _bookStoreJob.ContinuationJobs();
        }

        public async Task UowWork()
        {
            await _bookStoreJob.UnitOfWorkJobs(new BookJobArgs
            {
                BookId = new Guid("53ADE93E-5B6E-407C-A0D2-9ED0C05EDA29")
            });
        }
    }
}