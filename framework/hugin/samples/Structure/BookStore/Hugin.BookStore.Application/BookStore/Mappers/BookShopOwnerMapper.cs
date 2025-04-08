using AutoMapper;
using Hugin.BookStore.Dtos;

namespace Hugin.BookStore.Mappers
{
    public class BookShopOwnerMapper : Profile
    {
        public BookShopOwnerMapper()
        {
            CreateMap<BookShopOwner, BookShopOwnerDto>();
        }
    }
}
