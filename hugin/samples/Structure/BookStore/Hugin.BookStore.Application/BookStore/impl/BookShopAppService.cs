using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hugin.BookStore.Daos;
using Hugin.BookStore.Dtos;
using Hugin.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace Hugin.BookStore.impl
{
    [SwaggerTag("Hugin Crud")]
    [Authorize(BookStorePermissions.BookShop.Default)]
    //[ExposeServices(typeof(IBookShopDaoService), IncludeDefaults = true, IncludeSelf = true)]   //重写模块，不要这么蠢的方式
    public class BookShopAppService
        : BaseCrudStopAppService<BookShop, Guid, BookShopDto, BookShopQueryInput, BookShopEditDto, BookShopEditOutput, BookShopEditInput>,
            IBookShopAppService, IBookShopDaoService
    {
        private readonly IRepository<BookShop, Guid> _bookShopRepository;
        private readonly IRepository<BookInBookShop, Guid> _bookInBookShopRepository;
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IRepository<Author, Guid> _authorRepository;
        private readonly IRepository<BookShopOwner, Guid> _bookShopOwnerRepository;
        private readonly IBookDaoService _bookDaoService;
        private readonly IDataFilter _dataFilter;

        public BookShopAppService(IRepository<BookShop, Guid> bookShopRepository,
            IRepository<BookInBookShop, Guid> bookInBookShopRepository,
            IRepository<Book, Guid> bookRepository,
            IRepository<Author, Guid> authorRepository,
            IRepository<BookShopOwner, Guid> bookShopOwnerRepository,
            IBookDaoService bookDaoService,
            IDataFilter dataFilter) : base(bookShopRepository)
        {
            _bookShopRepository = bookShopRepository;
            _bookInBookShopRepository = bookInBookShopRepository;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _bookShopOwnerRepository = bookShopOwnerRepository;
            _bookDaoService = bookDaoService;
            _dataFilter = dataFilter;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BookStorePermissions.BookShop.Create)]
        [RemoteService]
        public override Task<BookShopDto> CreateAsync(BookShopEditInput input)
        {
            return base.CreateAsync(input);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BookStorePermissions.BookShop.Edit)]
        [RemoteService]
        public override Task<BookShopDto> UpdateAsync(Guid id, BookShopEditInput input)
        {
            return base.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(BookStorePermissions.BookShop.Delete)]
        [RemoteService]
        public override Task DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(BookStorePermissions.BookShop.Stop)]
        [RemoteService]
        public override Task Stop(Guid id)
        {
            return base.Stop(id);
        }

        /// <summary>
        /// 取消停用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(BookStorePermissions.BookShop.Stop)]
        [RemoteService]
        public override Task CancelStop(Guid id)
        {
            return base.CancelStop(id);
        }

        protected override IQueryable<object> CreateDefaultQuery()
        {
            return QueryBookShop();
        }

        public IQueryable<BookShopDao> QueryBookShop()
        {
            var query = from bookShop in Repository

                        join bookshopOwner in _bookShopOwnerRepository
                            on bookShop.OwnerId equals bookshopOwner.Id into g
                        from bookshopOwner in g.DefaultIfEmpty()

                        select new BookShopDao
                        {
                            BookShop = bookShop,
                            Books = (from bookInBookShop in _bookInBookShopRepository
                                     where bookInBookShop.BookShopId == bookShop.Id
                                     join bookDao in _bookDaoService.QueryBook()
                                         on bookInBookShop.BookId equals bookDao.Book.Id
                                     select bookDao).ToList(),
                            BookShopOwner = bookshopOwner
                        };

            return query;
        }

        #region 一些mapper尝试

        //[HttpGet]
        //[Route("/api/sample/book-shop/test/from-list")]
        //public async Task<List<BookShopDto>> LeftJoin()
        //{
        //    var query = from bookShop in Repository

        //                join bookshopOwner in _bookShopOwnerRepository
        //                    on bookShop.OwnerId equals bookshopOwner.Id into g
        //                from bookshopOwner in g.DefaultIfEmpty()

        //                let bookshopOwner2 = _bookShopOwnerRepository.FirstOrDefault(p => p.Id == bookShop.OwnerId) ?? new BookShopOwner()

        //                select new BookShopDto
        //                {
        //                    Id = bookShop.Id,
        //                    Code = bookShop.Code,
        //                    OwnerName = bookshopOwner2.Name
        //                };

        //    query = query.Where(p => p.Name.Contains("jd") || p.OwnerName.Contains("jd"));

        //    var entities = await AsyncExecuter.ToListAsync(query);

        //    return entities;
        //}

        //[HttpGet]
        //[Route("/api/sample/book-shop/test/from-list")]
        //public async Task<PagedResultDto<BookShopDto>> FromListMapperList(BookShopQueryInput input)
        //{
        //    var query = (IQueryable<BookShop>)Repository;
        //    query = ApplyFiltering(query, input);
        //    query = ApplySorting(query, input);
        //    query = ApplyPaging(query, input);

        //    var totalCount = await AsyncExecuter.CountAsync(query);
        //    var entities = await AsyncExecuter.ToListAsync(query);
        //    var entityDtos = ObjectMapper.Map<List<BookShop>, List<BookShopDto>>(entities);

        //    return new PagedResultDto<BookShopDto>(
        //        totalCount,
        //        entityDtos
        //    );
        //}

        //[HttpGet]
        //[Route("/api/sample/book-shop/test/from-list2")]
        //public async Task<PagedResultDto<BookShopDto>> FromListMapperList2(BookShopQueryInput input)
        //{
        //    /*
        //     *  只是简单的封装，实现对象-对象的的映射
        //     *  但同时，排序、模糊查询非常不友好，也不能复用Dao层建立的查询
        //     */

        //    var query0 = (IQueryable<BookShop>)_bookShopRepository;
        //    query0 = ApplyFiltering(query0, input);
        //    query0 = ApplySorting(query0, input);

        //    var query = from bookShop in query0

        //                join bookshopOwner in _bookShopOwnerRepository
        //                       on bookShop.OwnerId equals bookshopOwner.Id into g
        //                from bookshopOwner in g.DefaultIfEmpty()

        //                select new BookShopDao
        //                {
        //                    BookShop = bookShop,
        //                    Books = (from bookInBookShop in _bookInBookShopRepository
        //                             where bookInBookShop.BookShopId == bookShop.Id
        //                             join bookDao in _bookDaoService.QueryBook()
        //                                 on bookInBookShop.BookId equals bookDao.Book.Id
        //                             select bookDao).ToList(),
        //                    BookShopOwner = bookshopOwner
        //                };

        //    //这还只能写在这里，因为可能会被筛掉一些数据
        //    query = ApplyPaging(query, input);

        //    var totalCount = await AsyncExecuter.CountAsync(query);
        //    var entities = await AsyncExecuter.ToListAsync(query);
        //    var entityDtos = ObjectMapper.Map<List<BookShopDao>, List<BookShopDto>>(entities);

        //    return new PagedResultDto<BookShopDto>(
        //        totalCount,
        //        entityDtos
        //    );
        //}

        //[HttpGet]
        //[Route("/api/sample/book-shop/test/from-linq")]
        //public async Task<PagedResultDto<BookShopDto>> Test_LinqListThenMapper(BookShopQueryInput input)
        //{
        //    /*
        //     *  Linq进行对象转换，再针对对象排序、筛选
        //     *  必须注意的筛选限制是筛选字段必须是数据库字段，同时因为只映射了一层数据，所以包含的子对象也不能被查询
        //     *  同样的排序字段也必须是数据库字段，可以是子对象字段
        //     */

        //    //在使用ProjectTo时，尽可能不附加继续处理的表达式，因为它在数据库执行，会影响效率
        //    var query = MapperAccessor.Mapper.ProjectTo<BookShopDto>(QueryBookShop());

        //    query = ApplyFiltering(query, input);
        //    query = ApplySorting(query, input);
        //    query = ApplyPaging(query, input);

        //    var totalCount = await AsyncExecuter.CountAsync(query);
        //    var entityDtos = await AsyncExecuter.ToListAsync(query);

        //    return new PagedResultDto<BookShopDto>(
        //        totalCount,
        //        entityDtos
        //    );
        //}

        #endregion
    }
}
