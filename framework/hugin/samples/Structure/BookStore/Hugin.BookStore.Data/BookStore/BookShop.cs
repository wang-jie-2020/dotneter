using Hugin.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.MultiTenancy;

namespace Hugin.BookStore
{
    /// <summary>
    /// 书店
    /// </summary>
    public class BookShop : StopFullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        [Description("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// Boss
        /// </summary>
        [Description("Boss")]
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// 提供私有构造函数以实现序列化
        /// </summary>
        private BookShop()
        {

        }

        /// <summary>
        /// 提供公共构造函数以实现约束
        /// </summary>
        public BookShop(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
