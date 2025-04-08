using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Diagnostics;
using Dapper;
using Demo.Uow;
using MySqlConnector;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;
using UnitOfWorkContextRepository.Transaction;

namespace Demo.Controllers
{
    [ApiController]
    [Route("unit-of-work")]
    public class UnitOfWorkController : ControllerBase
    {
        private readonly IEfCoreRepository _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkController(IEfCoreRepository repository, IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
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
                Name = "Douglas Adams",
                BirthDate = new DateTime(1952, 03, 11),
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
                PublishDate = new DateTime(1995, 9, 27),
                Price = 42.0m
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

        [Route("list")]
        [HttpGet]
        public async Task<object> ListAsync()
        {
            var query1 = await _repository.GetQueryableAsync<Author>();
            var query2 = await _repository.GetQueryableAsync<Book>();
            return new
            {
                list1 = await query1.ToListAsync(),
                list2 = await query2.ToListAsync(),
            };
        }


        [Route("database-transaction")]
        [HttpGet]
        public async Task<object> DataBaseTransactionAsync()
        {
            var context = await _repository.GetDbContextAsync();
            using (var trans =
                   await context.Database.BeginTransactionAsync(isolationLevel: IsolationLevel.ReadCommitted))
            {
                try
                {
                    var query1 = await _repository.GetQueryableAsync<Author>();
                    var list1 = await query1.ToListAsync();

                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);
                    await context.SaveChangesAsync();

                    await trans.CommitAsync();
                }
                catch
                {
                    await trans.RollbackAsync();
                }
            }

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.ToListAsync();
        }

        [Route("database-transaction-error")]
        [HttpGet]
        public async Task<object> DatabaseTransactionErrorAsync()
        {
            var context = await _repository.GetDbContextAsync();
            using (var trans =
                   await context.Database.BeginTransactionAsync(isolationLevel: IsolationLevel.ReadCommitted))
            {
                try
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);
                    await context.SaveChangesAsync();

                    throw new Exception("manual error");

                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                }
            }

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.ToListAsync();
        }

        [Route("database-transaction-conflict")]
        [HttpGet]
        public async Task<object> DatabaseTransactionConflictAsync()
        {
            var context = await _repository.GetDbContextAsync();

            var connection = new MySqlConnection(context.Database.GetConnectionString());
            connection.Open();

            using (var dapperTrans = await connection.BeginTransactionAsync())
            {
                await connection.ExecuteAsync("select * from author for update", null, dapperTrans);

                using (var trans =
                       await context.Database.BeginTransactionAsync(isolationLevel: IsolationLevel.ReadCommitted))
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);
                    await context.SaveChangesAsync();
                    await trans.CommitAsync();
                }

                await dapperTrans.CommitAsync();
            }

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.ToListAsync();
        }

        [Route("uow-transaction")]
        [HttpGet]
        public async Task<object> UowTransactionAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);

                    await uow.CompleteAsync();
                }
                catch
                {

                }
            }

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.ToListAsync();
        }

        [Route("uow-transaction-error")]
        [HttpGet]
        public async Task<object> UowTransactionErrorAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);

                    throw new Exception("manual error");
                    await uow.CompleteAsync();
                }
                catch
                {

                }
            }

            var query = await _repository.GetQueryableAsync<Author>();
            return await query.ToListAsync();
        }

        //注意:以下是以直接抛出的Exception,在try-catch的处理不同时,结果也会有不同

        [Route("uow-in-uow-inner-error")]
        [HttpGet]
        public async Task<object> UowInUowInnerErrorAsync()
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
            return new
            {
                list1 = await query1.ToListAsync(),
                list2 = await query2.ToListAsync(),
            };
        }

        [Route("uow-in-uow-outer-error")]
        [HttpGet]
        public async Task<object> UowInUowOuterErrorAsync()
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
            return new
            {
                list1 = await query1.ToListAsync(),
                list2 = await query2.ToListAsync(),
            };
        }

        /// <summary>
        ///  注意:不要用SQLITE内存测试
        /// </summary>
        /// <returns></returns>
        [Route("uow-over-uow-inner-error")]
        [HttpGet]
        public async Task<object> UowOverUowInnerErrorAsync()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);

                    using (var uow1 = _unitOfWorkManager.Begin(new UnitOfWorkOptions(), requiresNew: true))
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
            return new
            {
                list1 = await query1.ToListAsync(),
                list2 = await query2.ToListAsync(),
            };
        }

        /// <summary>
        ///  注意:不要用SQLITE内存测试
        /// </summary>
        /// <returns></returns>
        [Route("uow-over-uow-outer-error")]
        [HttpGet]
        public async Task<object> UowOverUowOuterErrorAsync()
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    await _repository.InsertAsync(authors[0]);
                    await _repository.InsertAsync(authors[1]);

                    using (var uow1 = _unitOfWorkManager.Begin(new UnitOfWorkOptions(), requiresNew: true))
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
            return new
            {
                list1 = await query1.ToListAsync(),
                list2 = await query2.ToListAsync(),
            };
        }

        [Route("uow-filter")]
        [HttpGet]
        [UnitOfWork]
        public async Task UowFilterAsync()
        {
            await _repository.InsertAsync(authors[0]);
            await _repository.InsertAsync(authors[1]);
            await _repository.InsertAsync(books[0]);
            await _repository.InsertAsync(books[1]);
        }

        [Route("uow-filter-error")]
        [HttpGet]
        [UnitOfWork]
        public async Task UowFilterErrorAsync()
        {
            await _repository.InsertAsync(authors[0]);
            await _repository.InsertAsync(authors[1]);

            throw new Exception("manual error");

            await _repository.InsertAsync(books[0]);
            await _repository.InsertAsync(books[1]);
        }

        [Route("uow-filter-error-handled")]
        [HttpGet]
        [UnitOfWork]
        public async Task UowFilterErrorHandledAsync()
        {
            await _repository.InsertAsync(authors[0]);
            await _repository.InsertAsync(authors[1]);

            try
            {
                throw new Exception("manual error");

                await _repository.InsertAsync(books[0]);
                await _repository.InsertAsync(books[1]);
            }
            catch
            {
                //ignored
            }
        }
    }
}