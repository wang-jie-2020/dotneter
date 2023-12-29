using System;
using System.ComponentModel;
using Hugin.BookStore.Enums;
using Volo.Abp.Application.Dtos;

namespace Hugin.BookStore.Dtos
{
    //public class BookOutput : IHoused<BookDto>
    //{
    //    public BookDto Item { get; set; }
    //}

    public class BookDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
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
        [DisplayName("作者Id")]
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public AuthorDto Author { get; set; }
    }
}