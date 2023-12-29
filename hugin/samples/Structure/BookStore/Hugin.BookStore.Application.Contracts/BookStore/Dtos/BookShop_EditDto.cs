using System;
using System.ComponentModel.DataAnnotations;
using Hugin.Application.Dtos;

namespace Hugin.BookStore.Dtos
{
    public class BookShopEditOutput : IHoused<BookShopEditDto>
    {
        public BookShopEditDto Item { get; set; }
    }

    public class BookShopEditInput : IHoused<BookShopEditDto>
    {
        [Required]
        public BookShopEditDto Item { get; set; }
    }

    public class BookShopEditDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        public string Name { get; set; }

        /// <summary>
        /// Boss
        /// </summary>
        public Guid? OwnerId { get; set; }
    }
}