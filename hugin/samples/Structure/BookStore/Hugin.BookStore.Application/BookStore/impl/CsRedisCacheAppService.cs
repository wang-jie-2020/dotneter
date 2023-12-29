using System.Linq;
using System.Threading.Tasks;
using Hugin.BookStore.Ctos;
using Hugin.BookStore.Daos;
using Hugin.BookStore.Dtos;
using Hugin.Cache.CsRedis;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Hugin.BookStore.impl
{
    [SwaggerTag("CsRedis Cache")]
    public class CsRedisCacheAppService : BaseAppService
    {
        private readonly IBookDaoService _bookDaoService;
        private readonly IBookShopDaoService _bookShopDaoService;
        private readonly ICsRedisCache<BookDto> _bookCache;
        private readonly ICsRedisCache<BookShopCto> _bookShopCache;

        public CsRedisCacheAppService(IBookDaoService bookDaoService,
            IBookShopDaoService bookShopDaoService,
            ICsRedisCache<BookDto> bookCache,
            ICsRedisCache<BookShopCto> bookShopCache)
        {
            _bookDaoService = bookDaoService;
            _bookShopDaoService = bookShopDaoService;
            _bookCache = bookCache;
            _bookShopCache = bookShopCache;
        }

        public async Task<object> GetSetCache()
        {
            var query = MapperAccessor.Mapper.ProjectTo<BookDto>(_bookDaoService.QueryBook());
            var book = await query.FirstOrDefaultAsync();

            await _bookCache.SetAsync(book.Id.ToString(), book);
            return await _bookCache.GetAsync(book.Id.ToString());
        }

        public async Task<object> GetOrAddCache()
        {
            var query = MapperAccessor.Mapper.ProjectTo<BookShopCto>(_bookShopDaoService.QueryBookShop());
            var bookShop = await query.FirstOrDefaultAsync();

            var cachedBookShop = await _bookShopCache.GetOrAddAsync(bookShop.BookShop.Id.ToString(),
                async () => await query.FirstOrDefaultAsync());

            return await _bookShopCache.GetAsync(bookShop.BookShop.Id.ToString());
        }

        public async Task<object> SetHashCache()
        {
            var query = MapperAccessor.Mapper.ProjectTo<BookDto>(_bookDaoService.QueryBook());
            var books = await query.ToListAsync();

            await _bookCache.HSetAsync(books.Select(p => p.Id.ToString()).ToArray(), books.ToArray());
            return await _bookCache.HGetAsync(books.Select(p => p.Id.ToString()).ToArray());
        }

        public async Task<object> GetOrAddHashCache()
        {
            var query = MapperAccessor.Mapper.ProjectTo<BookShopCto>(_bookShopDaoService.QueryBookShop());
            var bookShops = await query.ToListAsync();

            var cachedBookShop = await _bookShopCache.HGetOrAddAsync(bookShops.Select(p => p.BookShop.Id.ToString()).ToArray(),
                async () => (await query.ToListAsync()).ToArray());

            return await _bookShopCache.HGetAsync(bookShops.Select(p => p.BookShop.Id.ToString()).ToArray());
        }
    }
}