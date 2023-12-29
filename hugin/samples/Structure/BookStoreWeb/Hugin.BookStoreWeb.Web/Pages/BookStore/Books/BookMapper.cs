using AutoMapper;
using Hugin.BookStore.Dtos;

namespace Hugin.BookStoreWeb.Web.Pages.BookStore.Books
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<BookDto, BookEditDto>();
            CreateMap<CreateModalModel.CreateBookViewModel, BookEditDto>();
            CreateMap<BookDto, EditModalModel.EditBookViewModel>();
            CreateMap<EditModalModel.EditBookViewModel, BookEditDto>();
        }
    }
}
