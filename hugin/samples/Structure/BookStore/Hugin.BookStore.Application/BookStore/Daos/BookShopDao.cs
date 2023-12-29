using System.Collections.Generic;

namespace Hugin.BookStore.Daos
{
    public class BookShopDao
    {
        public BookShop BookShop { get; set; }

        public IEnumerable<BookDao> Books { get; set; }

        public BookShopOwner BookShopOwner { get; set; }
    }
}
