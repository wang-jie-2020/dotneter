using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Hugin.BookStore.impl
{
    [SwaggerTag("Cap Event")]
    public class CapAppService : BaseAppService
    {
        private readonly IBookStoreEvent _bookStoreEvent;

        public CapAppService(IBookStoreEvent bookStoreEvent)
        {
            _bookStoreEvent = bookStoreEvent;
        }

        public async Task PublishEventAsync()
        {
            await _bookStoreEvent.PublishBookStoreEventAsync();
        }
    }
}