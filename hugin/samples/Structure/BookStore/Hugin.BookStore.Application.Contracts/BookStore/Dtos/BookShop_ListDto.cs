using System;
using System.Collections.Generic;
using System.Linq;
using Hugin.Application.Dtos;
using Hugin.Application.Filters;
using Hugin.Infrastructure.Extensions;

namespace Hugin.BookStore.Dtos
{
    //public class BookShopOutput : IHoused<BookShopDto>
    //{
    //    public BookShopDto Item { get; set; }
    //}

    public class BookShopDto : DefaultStopDto<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        [QueryFilterField]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [QueryFilterField]
        public string Name { get; set; }

        /// <summary>
        /// 售卖书籍
        /// </summary>
        public IEnumerable<BookDto> Books { get; set; }

        /// <summary>
        /// Boss
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// BossName 
        /// </summary>
        [QueryFilterField]
        public string OwnerName { get; set; }

        /// <summary>
        /// Boss 
        /// </summary>
        public BookShopOwnerDto BookShopOwner { get; set; }

        /// <summary>
        /// BooksOnSale 是Dto属性，而不是数据库字段，不能作为查询筛选字段
        /// 想要查询必须指定对应关系，如OwnerName
        /// </summary>
        public string BooksOnSale => Books.IsEmpty()
             ? string.Empty
             : Books.Select(p => p.Name).ToList().Aggregate((a, b) => a + "," + b); //string.Join(',', Books.Select(p => p.Name));
    }
}