using AutoMapper;
using Hugin.BookStore.Daos;
using Hugin.BookStore.Dtos;

namespace Hugin.BookStore.Mappers
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<Book, BookDto>();
            CreateMap<Book, BookEditDto>();
            CreateMap<BookEditDto, Book>();

            CreateMap<BookDao, BookDto>().IncludeMembers(s => s.Book);
        }
    }
}
