using System;
using System.Threading;
using System.Threading.Tasks;
using Hugin.BookStore.Etos;

namespace Hugin.BookStore.impl
{

    public class BookStoreEvent : BaseCapEvent, IBookStoreEvent
    {
        public BookStoreEvent()
        {
        }

        public async Task PublishBookStoreEventAsync()
        {
            await CapPublisher.PublishAsync(EventNameConsts.BookStoreEvent, new BookShopEventArgs
            {
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                BookId = Guid.NewGuid(),
                BookShopId = Guid.NewGuid()
            });
        }
    }
}