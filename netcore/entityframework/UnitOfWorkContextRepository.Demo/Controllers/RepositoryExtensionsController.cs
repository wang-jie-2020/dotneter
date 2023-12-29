using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Demo.Controllers
{
    [ApiController]
    [Route("repository-extensions")]
    public class RepositoryExtensionsController : ControllerBase
    {
        private readonly IEfCoreRepository _repository;

        public RepositoryExtensionsController(IEfCoreRepository repository)
        {
            _repository = repository;
        }

        [Route("insert")]
        [HttpGet]
        public async Task<object> InsertManyAsync()
        {
            var list = new List<Author>();
            for (int i = 0; i < 5000; i++)
            {
                list.Add(new Author()
                {
                    Name = i.ToString()
                });
            }

            await _repository.InsertManyAsync(list);
            await _repository.SaveChangesAsync();

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.CountAsync();
        }

        [Route("insert-bulk")]
        [HttpGet]
        public async Task<object> BulkInsertManyAsync()
        {
            //var list = new List<Author>();
            //for (int i = 0; i < 5000; i++)
            //{
            //    list.Add(new Author()
            //    {
            //        Name = i.ToString()
            //    });
            //}

            //await _repository.BulkInsertAsync(list);

            //var query = await _repository.GetQueryableAsync<Author>();
            //return await query.CountAsync();


            var list1 = new List<Author>();
            for (int i = 0; i < 5000; i++)
            {
                list1.Add(new Author()
                {
                    Name = i.ToString()
                });
            }

            var list2 = new List<Book>();
            for (int i = 0; i < 5000; i++)
            {
                list2.Add(new Book()
                {
                    Name = i.ToString()
                });
            }

            var list3 = new List<BookAuthor>();
            for (int i = 0; i < 5000; i++)
            {
                list3.Add(new BookAuthor()
                {
                    Author = list1[i],
                    Book = list2[i]
                });
            }

            await _repository.BulkInsertAsync(list3.Select(a => a.Book).ToList(), new BulkConfig()
            {
                SetOutputIdentity = true
            });
            //await _context.BulkInsertAsync(list2, config => config.SetOutputIdentity = true);


            var query = await _repository.GetQueryableAsync<Author>();
            return await query.CountAsync();
        }

        [Route("batch")]
        [HttpGet]
        public async Task<object> BatchAsync()
        {
            await _repository.BatchUpdateAsync<Author>(a => a.Id < 5, b => new Author
            {
                Name = "update name"
            });

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.Skip(0).Take(10).ToListAsync();
        }

        internal class BookAuthor
        {
            public Book Book { get; set; }

            public Author Author { get; set; }
        }
    }
}