using System.Threading.Tasks;

namespace Hugin.BookStore
{
    public interface IBookStoreEvent
    {
        Task PublishBookStoreEventAsync();
    }
}

