using AutoMapper;
using Demo.Models;
using Demo.ViewModels;

namespace Demo.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserExtend, UserExtendDto>();

            CreateMap<UserDao, UserDto>()
                .IncludeMembers(s => s.User);
        }
    }
}
