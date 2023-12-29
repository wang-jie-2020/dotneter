using Hugin.Domain.Entities.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hugin.BookStore.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Hugin.BookStore
{
    /// <summary>
    /// 书籍
    /// </summary>
    public class Book : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型")]
        public BookType BookType { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [Description("出版日期")]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DecimalPrecision]
        [Description("价格")]
        public decimal Price { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        [Description("作者Id")]
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        [Description("封面")]
        public string CoverUrl { get; set; }
    }
}
