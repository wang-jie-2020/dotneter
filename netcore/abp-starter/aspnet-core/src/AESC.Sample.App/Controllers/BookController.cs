using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using AESC.Sample.Entities;
using AESC.Sample.Permissions;
using AESC.Utils.AbpExtensions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace AESC.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : SampleController
    {
        private readonly IRepository<Book> _repository;

        protected DbContext Context => _repository.GetDbContext();

        public BookController(IRepository<Book> repository)
        {
            _repository = repository;
        }

        [HttpGet("create")]
        public async Task Create(string name)
        {
            var book = new Book()
            {
                Name = name,
                UserId = CurrentUser.Id ?? throw new Exception("never")
            };
            book.SetId();
            await _repository.InsertAsync(book);
        }

        [HttpGet("list")]
        public async Task<object> List()
        {
            return await _repository.GetListAsync();
        }

        [HttpGet("page")]
        public async Task<object> Page()
        {
            var query = from book in Context.Set<Book>()
                        join user in Context.Set<AppUser>()
                            on book.UserId equals user.Id into g
                        from userJoined in g.DefaultIfEmpty()
                        select new
                        {
                            Book = book,
                            User = userJoined
                        };

            return await query.PageBy(0, 10).ToListAsync();
        }
    }
}
