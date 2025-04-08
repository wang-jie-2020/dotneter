using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hugin.Application.Dtos;
using Hugin.Application.Validates;
using Hugin.BookStore.Enums;

namespace Hugin.BookStore.Dtos
{
    public class BookEditOutput : IHoused<BookEditDto>
    {
        public BookEditDto Item { get; set; }
    }

    public class BookEditInput : IHoused<BookEditDto>
    {
        [Required]
        public BookEditDto Item { get; set; }
    }

    public class BookEditDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        [DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName("类型")]
        public BookType BookType { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [DisplayName("出版日期")]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DisplayName("价格")]
        public decimal Price { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        [NotDefault]
        [DisplayName("作者Id")]
        public Guid? AuthorId { get; set; }
    }
}