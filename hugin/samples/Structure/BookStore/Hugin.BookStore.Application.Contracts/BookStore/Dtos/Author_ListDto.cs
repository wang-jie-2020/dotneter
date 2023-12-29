using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Hugin.BookStore.Dtos
{
    //public class AuthorOutput : IHoused<AuthorDto>
    //{
    //    public AuthorDto Item { get; set; }
    //}

    public class AuthorDto : EntityDto<Guid>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DisplayName("出生日期")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        [DisplayName("个人简介")]
        public string Profile { get; set; }
    }
}