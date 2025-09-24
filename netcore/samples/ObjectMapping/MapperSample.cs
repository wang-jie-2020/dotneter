using AutoMapper;
using Mapster;
using Mapster.Utils;
using Microsoft.Extensions.Logging;

namespace ObjectMapping;

public class MapperSample
{
    void Method1()
    {
        LoggerFactory loggerFactory = new LoggerFactory();
        
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<Role, RoleDto>();
        }, loggerFactory);

        var mapper = config.CreateMapper();
        
        var user = new User()
        {
            Name = "admin",
            Roles = new List<Role>()
            {
                new Role() { Name = "admin" },
                new Role() { Name = "default" }
            }
        };
        
        var userDto = mapper.Map<UserDto>(user);
        Console.WriteLine(userDto);
    }

    void Method2()
    {
        var user = new User()
        {
            Name = "admin",
            Roles = new List<Role>()
            {
                new Role() { Name = "admin" },
                new Role() { Name = "default" }
            }
        };

        var userDto = user.Adapt<UserDto>();
        Console.WriteLine(userDto);
        
        /*  一种写配置的方法
         *  var config = new TypeAdapterConfig();
            //映射规则
            config.ForType<User, UserDto>()
                .Map(dest => dest.UserAge, src => src.Age)
                .Map(dest => dest.UserSex, src => src.Sex);

            var mapper = new Mapper(config);//务必将mapper设为单实例

            var user = new User{Name = "xiaowang",Age = 18,Sex = "boy"};
            var dto = mapper.Map<UserDto>(user);
         */
    }
}