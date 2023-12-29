using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities;

namespace Hugin.BookStore
{
    /// <summary>
    /// 书店售卖书籍
    /// </summary>
    public class BookInBookShop : Entity<Guid>
    {
        /// <summary>
        /// 书籍Id
        /// </summary>
        [Description("书籍Id")]
        public Guid BookId { get; set; }

        /// <summary>
        /// 书店Id
        /// </summary>
        [Description("书店Id")]
        public Guid BookShopId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int Num { get; set; }
    }
}
