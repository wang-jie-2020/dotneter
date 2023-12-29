using System;
using System.Linq;
using Hugin.BookStore.Daos;
using Hugin.BookStore.Dtos;
using Hugin.BookStore.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Hugin.BookStore.impl
{
    [ApiExplorerSettings(GroupName = ApiGroups.BookStore)]
    [SwaggerTag("Abp Crud")]
    public class BookAppService
        : CrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto, BookEditDto>,
            IBookAppService, IBookDaoService
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IRepository<Author, Guid> _authorRepository;

        public BookAppService(IRepository<Book, Guid> bookRepository, IRepository<Author, Guid> authorRepository) : base(bookRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;

            //这样处理权限非常不直观，也不利于重写
            GetPolicyName = BookStorePermissions.Book.Default;
            GetListPolicyName = BookStorePermissions.Book.Default;
            CreatePolicyName = BookStorePermissions.Book.Create;
            UpdatePolicyName = BookStorePermissions.Book.Edit;
            DeletePolicyName = BookStorePermissions.Book.Delete;
        }

        public IQueryable<BookDao> QueryBook()
        {
            var query = from book in ApplyDefaultSorting(_bookRepository)
                        join author in _authorRepository
                            on book.AuthorId equals author.Id into authorJoined
                        from author in authorJoined.DefaultIfEmpty()
                        select new BookDao
                        {
                            Book = book,
                            Author = author
                        };

            return query;
        }
    }
}
