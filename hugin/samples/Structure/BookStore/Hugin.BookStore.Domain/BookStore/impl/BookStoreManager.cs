using System;
using System.Threading.Tasks;
using Hugin.BookStore.Etos;
using Volo.Abp.EventBus;

namespace Hugin.BookStore.impl
{
    public class BookStoreManager : BaseDomainManager,
        IBookStoreManager,
        ILocalEventHandler<BookEventArgs>,
        ILocalEventHandler<BookErrorEventArgs>
    {
        public async Task HandleEventAsync(BookEventArgs eventData)
        {
            await Task.CompletedTask;
        }

        public async Task HandleEventAsync(BookErrorEventArgs eventData)
        {
            await Task.CompletedTask;
            throw new Exception("Book Error");
        }
    }
}
