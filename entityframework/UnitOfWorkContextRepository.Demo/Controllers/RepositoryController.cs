using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;
using static System.Reflection.Metadata.BlobBuilder;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Demo.Controllers
{
    [ApiController]
    [Route("repository")]
    public class RepositoryController : ControllerBase
    {
        private readonly IEfCoreRepository _repository;

        public RepositoryController(IEfCoreRepository repository)
        {
            _repository = repository;
        }

        private Author[] authors = new Author[]
        {
            new Author()
            {
                Name = "George Orwell",
                BirthDate = new DateTime(1903, 06, 25),
                Profile =
                    "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
            },
            new Author()
            {
                Name =   "Douglas Adams",
                BirthDate =  new DateTime(1952, 03, 11),
                Profile =
                    "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
            }
        };

        private Book[] books = new Book[]
        {
            new Book()
            {
                Name = "1984",
                PublishDate = new DateTime(1949, 6, 8),
                Price = 19.84m,
            },
            new Book()
            {
                Name = "The Hitchhiker's Guide to the Galaxy",
                PublishDate =new DateTime(1995, 9, 27),
                Price =  42.0m
            },
            new Book()
            {
                Name = "剑来",
                PublishDate = new DateTime(2019, 1, 1),
                Price = 65.0m
            },
            new Book()
            {
                Name = "大道朝天",
                PublishDate = new DateTime(2019, 1, 1),
                Price = 75.0m
            }
        };

        [Route("insert-update-delete")]
        [HttpGet]
        public async Task<object> InsertUpdateAsync()
        {
            await _repository.InsertAsync(authors[0]);
            await _repository.InsertAsync(authors[1]);
            await _repository.SaveChangesAsync();

            authors[0].Name = "updated name";
            await _repository.UpdateAsync(authors[0]);
            await _repository.SaveChangesAsync();

            await _repository.HardDeleteAsync(authors[1]);
            await _repository.SaveChangesAsync();

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.ToListAsync();
        }

        [Route("insert-update-delete-many")]
        [HttpGet]
        public async Task<object> InsertUpdateManyAsync()
        {
            await _repository.InsertManyAsync(books);
            await _repository.SaveChangesAsync();

            books[0].Name = "updated name";
            await _repository.UpdateAsync(books[0]);
            await _repository.SaveChangesAsync();

            await _repository.HardDeleteManyAsync(new Book[] { books[2], books[3] });
            await _repository.SaveChangesAsync();

            var query = await _repository.GetQueryableAsync<Book>();
            return await query.ToListAsync();
        }
    }
}