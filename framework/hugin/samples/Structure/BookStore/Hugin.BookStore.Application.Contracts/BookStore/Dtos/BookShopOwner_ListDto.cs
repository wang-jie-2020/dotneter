using System;
using Volo.Abp.Application.Dtos;

namespace Hugin.BookStore.Dtos
{
    public class BookShopOwnerDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}