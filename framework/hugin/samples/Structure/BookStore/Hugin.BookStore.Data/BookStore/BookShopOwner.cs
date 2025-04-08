using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Hugin.BookStore
{
    public class BookShopOwner : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(BookStoreConsts.EntityLengths.Default)]
        [Description("名称")]
        public string Name { get; set; }
    }
}
