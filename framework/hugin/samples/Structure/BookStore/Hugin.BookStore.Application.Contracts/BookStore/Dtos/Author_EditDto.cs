using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hugin.Application.Dtos;

namespace Hugin.BookStore.Dtos
{
    public class AuthorEditOutput : IHoused<AuthorEditDto>
    {
        public AuthorEditDto Item { get; set; }
    }

    public class AuthorEditInput : IHoused<AuthorEditDto>
    {
        [Required]
        public AuthorEditDto Item { get; set; }
    }

    public class AuthorEditDto : IValidatableObject
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(64)]
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
        [MaxLength(BookStoreConsts.EntityLengths.More)]
        [DisplayName("个人简介")]
        public string Profile { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == Profile)
            {
                //注意：这里还是需要本地化的
                yield return new ValidationResult(
                    "Name and Profile can not be the same!",
                    new[] { "Name", "Profile" }
                );
            }
        }
    }
}