using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    /// <summary>
    /// 书籍
    /// </summary>
    public class Book
    {
        //public Guid Id { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        //public Guid? AuthorId { get; set; }

        public int? AuthorId { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverUrl { get; set; }
    }
}
