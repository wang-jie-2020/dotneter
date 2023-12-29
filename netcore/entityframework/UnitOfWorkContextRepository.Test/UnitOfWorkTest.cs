using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Demo.Data;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;
using UnitOfWorkContextRepository.Test.Fixtures;
using UnitOfWorkContextRepository.Transaction;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UnitOfWorkContextRepository.Test
{
    [Collection("uow-collection")]
    public class UnitOfWorkTest
    {
        private readonly UowFixture _baseFixture;
        private readonly IEfCoreRepository _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkTest(UowFixture baseFixture)
        {
            _baseFixture = baseFixture;
            _repository = baseFixture.ServiceProvider.GetRequiredService<IEfCoreRepository>();
            _unitOfWorkManager = baseFixture.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
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

        [Fact]
        public async Task UowTransactionErrorTest()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);
                    throw new Exception("manual error");
                    await uow.CompleteAsync();
                }
            }
            catch (Exception e)
            {
                //ignored
            }

            var query = await _repository.GetQueryableAsync<Author>();
            var list = await query.ToListAsync();
            list.ShouldBeEmpty();
        }

        [Fact]
        public async Task NestUowTransactionInnerErrorTest()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);

                    using (var uow1 = _unitOfWorkManager.Begin())
                    {

                        await _repository.InsertAsync(books[0]);
                        await _repository.InsertAsync(books[1]);

                        throw new Exception("inner manual error");
                        await uow1.CompleteAsync();
                    }

                    await uow.CompleteAsync();
                }
            }
            catch (Exception e)
            {
                //ignored
            }

            var query1 = await _repository.GetQueryableAsync<Author>();
            var query2 = await _repository.GetQueryableAsync<Book>();

            var list1 = await query1.ToListAsync();
            var list2 = await query2.ToListAsync();

            list1.ShouldBeEmpty();
            list2.ShouldBeEmpty();
        }

        [Fact]
        public async Task NestUowTransactionOuterErrorTest()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);

                    using (var uow1 = _unitOfWorkManager.Begin())
                    {

                        await _repository.InsertAsync(books[0]);
                        await _repository.InsertAsync(books[1]);

                        await uow1.CompleteAsync();
                    }

                    throw new Exception("outer manual error");
                    await uow.CompleteAsync();
                }
            }
            catch (Exception e)
            {
                //ignored
            }

            var query1 = await _repository.GetQueryableAsync<Author>();
            var query2 = await _repository.GetQueryableAsync<Book>();

            var list1 = await query1.ToListAsync();
            var list2 = await query2.ToListAsync();

            list1.ShouldBeEmpty();
            list2.ShouldBeEmpty();
        }
    }
}
