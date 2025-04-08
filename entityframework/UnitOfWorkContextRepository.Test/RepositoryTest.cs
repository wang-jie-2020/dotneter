using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;
using UnitOfWorkContextRepository.Test.Fixtures;
using Xunit;

namespace UnitOfWorkContextRepository.Test
{
    public class RepositoryTest : IClassFixture<RepositoryFixture>
    {
        private readonly IEfCoreRepository _repository;

        public RepositoryTest(RepositoryFixture fixture)
        {
            _repository = fixture.ServiceProvider.GetRequiredService<IEfCoreRepository>();
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
        public async Task RepositoryCrudTest()
        {
            //read
            var query1 = await _repository.GetQueryableAsync<Author>();
            var query2 = await _repository.GetQueryableAsync<Book>();

            var list1 = await query1.ToListAsync();
            var list2 = await query2.ToListAsync();

            Assert.Empty(list1);
            Assert.Empty(list2);

            //create
            await _repository.InsertAsync(authors[0]);
            await _repository.InsertManyAsync(books);
            await _repository.SaveChangesAsync();

            list1 = await query1.ToListAsync();
            list2 = await query2.ToListAsync();

            Assert.NotEmpty(list1);
            Assert.NotEmpty(list2);

            //update
            authors[0].Name = "updated";
            foreach (var book in books)
            {
                book.Name = "updated";
            }
            await _repository.UpdateAsync(authors[0]);
            await _repository.UpdateManyAsync(books);
            await _repository.SaveChangesAsync();

            list1 = await query1.ToListAsync();
            list2 = await query2.ToListAsync();

            Assert.True(list1.First().Name == "updated");
            Assert.True(list2.All(t => t.Name == "updated"));


            //delete
            await _repository.HardDeleteAsync(authors[0]);
            await _repository.HardDeleteManyAsync(books);
            await _repository.SaveChangesAsync();

            list1 = await query1.ToListAsync();
            list2 = await query2.ToListAsync();

            Assert.Empty(list1);
            Assert.Empty(list2);
        }

        [Fact]
        public async Task RepositoryExtensionTest()
        {
            var query1 = await _repository.GetQueryableAsync<Author>();
            var query2 = await _repository.GetQueryableAsync<Book>();

            var list1 = await query1.ToListAsync();
            var list2 = await query2.ToListAsync();

            if (!list1.Any())
            {
                await _repository.BulkInsertAsync(authors);
            }

            if (!list2.Any())
            {
                await _repository.BulkInsertAsync(books);
            }

            list1 = await query1.ToListAsync();
            list2 = await query2.ToListAsync();

            Assert.NotEmpty(list1);
            Assert.NotEmpty(list2);

            await _repository.BatchUpdateAsync<Author>(t => true, author => new Author() { Name = "updated" });
            await _repository.BatchHardDeleteAsync<Book>(t => true);

            list1 = await query1.ToListAsync();
            list2 = await query2.ToListAsync();

            Assert.True(list1.All(t => t.Name == "updated"));
            Assert.Empty(list2);
        }
    }
}
