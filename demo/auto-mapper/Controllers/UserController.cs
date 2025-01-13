using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Data;
using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpGet]
        [Route("union-child-list")]
        public async Task<object> GetUnion()
        {
            var query = from user in _context.Users
                        select new UserDao()
                        {
                            User = user,
                            UserExtends = _context.UserExtends.Where(p => p.UserId == user.Id).ToList()
                        };
            var list = _mapper.Map<List<UserDto>>(await query.ToListAsync());

            /*
             *  EFCore==3.x时，报错，未将对象引用到对象的实例
             *  EFCore==5.x时，运行正常
             */
            var query2 = _mapper.ProjectTo<UserDto>(query);
            var list2 = await query2.ToListAsync();

            return new
            {
                MapperTo = list,
                ProjectTo = list2
            };
        }
    }
}


