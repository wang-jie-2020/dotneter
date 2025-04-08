using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hugin.BookStore.Etos;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Hugin.BookStore.impl
{
    /*
     * 这个例子演示的是本地事件操作
     */
    [SwaggerTag("Local Event")]
    public class LocalEventAppService : BaseAppService
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IRepository<BookShop, Guid> _bookShopRepository;

        public LocalEventAppService(IRepository<Book, Guid> bookRepository,
            IRepository<BookShop, Guid> bookShopRepository)
        {
            _bookRepository = bookRepository;
            _bookShopRepository = bookShopRepository;
        }

        public async Task EventPublishAsync()
        {
            var id = new Guid("53ADE93E-5B6E-407C-A0D2-9ED0C05EDA29");
            var book = await _bookRepository.FindAsync(id);
            book.Price = new Random().Next() * 1111.11m;
            await _bookRepository.UpdateAsync(book);

            await LocalEventBus.PublishAsync(
                new BookEventArgs
                {
                    BookId = id
                }
            );
        }

        /*
         *  若本地事件和调用者共享Uow,一旦任何点出现错误则都会整体回滚
         */
        public async Task EventPublishErrorAsync()
        {
            var id = new Guid("53ADE93E-5B6E-407C-A0D2-9ED0C05EDA29");
            var book = await _bookRepository.FindAsync(id);
            book.Price = new Random().Next() * 1111.11m;
            await _bookRepository.UpdateAsync(book);

            await LocalEventBus.PublishAsync(
                new BookEventArgs
                {
                    BookId = id
                }
            );

            await LocalEventBus.PublishAsync(
                new BookErrorEventArgs
                {
                    BookId = id
                }
            );
        }

        /*
         *  通常情况下不会共享Uow，Uow的设计在这里只是做一个简单例子
         *  需要注意的是两个Uow之间是不会共享上下文状态的
         */
        public async Task EventPublishHandlerErrorAsync()
        {
            var uowId = CurrentUnitOfWork.Id;

            var id = new Guid("53ADE93E-5B6E-407C-A0D2-9ED0C05EDA29");
            var book = await _bookRepository.FindAsync(id);
            book.Price = new Random().Next() * 1111.11m;
            await _bookRepository.UpdateAsync(book);

            await LocalEventBus.PublishAsync(
                new BookEventArgs
                {
                    BookId = id
                }
            );

            try
            {
                using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
                {
                    Debug.Assert(uow.Id != uowId);

                    var book2 = await _bookRepository.FindAsync(id);
                    Debug.Assert(book.Price != book2.Price);

                    await LocalEventBus.PublishAsync(
                        new BookErrorEventArgs
                        {
                            BookId = id
                        }
                    );
                }
            }
            catch
            {
                //throw or ignored
            }
        }
    }
}