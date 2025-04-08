using AutoMapper;
using Hugin.BookStore.Dtos;

namespace Hugin.BookStore.Mappers
{
    public class AuthorMapper : Profile
    {
        public AuthorMapper()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<Author, AuthorEditDto>();
            CreateMap<AuthorEditDto, Author>();
        }
    }
}
