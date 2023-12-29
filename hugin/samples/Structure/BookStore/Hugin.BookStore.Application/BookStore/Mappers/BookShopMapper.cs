using AutoMapper;
using Hugin.BookStore.Ctos;
using Hugin.BookStore.Daos;
using Hugin.BookStore.Dtos;

namespace Hugin.BookStore.Mappers
{
    public class BookShopMapper : Profile
    {
        public BookShopMapper()
        {
            CreateMap<BookShop, BookShopDto>();
            CreateMap<BookShop, BookShopEditDto>();
            CreateMap<BookShopEditDto, BookShop>();

            /*
             * 2021.12.20
             * OwnerName是通过LeftJoin关联查询的，可能存在不对应，在不对应的情况下是不能正常对表查询的，通过设置默认值可以解决
             */
            CreateMap<BookShopDao, BookShopDto>()
                .IncludeMembers(s => s.BookShop)
                .ForMember(m => m.OwnerName,
                    s => s.MapFrom(g => g.BookShopOwner.Name ?? string.Empty));

            CreateMap<BookShopCto, BookShopDao>();
            CreateMap<BookShopDao, BookShopCto>();
        }
    }
}
