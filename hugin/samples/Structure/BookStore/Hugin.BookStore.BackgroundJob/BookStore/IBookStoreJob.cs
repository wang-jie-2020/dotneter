using System.Threading.Tasks;
using Hugin.BookStore.Btos;

namespace Hugin.BookStore
{
    public interface IBookStoreJob
    {
        Task ImmediatelyJobs();

        Task DelayedJobs();

        Task RecurringJobs();

        Task ContinuationJobs();

        Task UnitOfWorkJobs(BookJobArgs args);
    }
}

