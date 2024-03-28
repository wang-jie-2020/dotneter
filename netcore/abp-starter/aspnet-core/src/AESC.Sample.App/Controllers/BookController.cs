using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AESC.Sample.Entities;

namespace AESC.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : SampleController
    {
        private readonly IRepository<Book> _repository;

        public BookController(IRepository<Book> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public object ListOwner()
        {
            var book = new Book();
     


            return _repository.GetListAsync().Result;
        }
    }
}
