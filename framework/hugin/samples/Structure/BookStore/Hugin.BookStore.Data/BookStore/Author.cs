using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Hugin.BookStore
{
    /// <summary>
    /// 作者
    /// </summary>
    public class Author : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(64)]
        [Description("姓名")]
        public string Name { get; private set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Description("出生日期")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        [MaxLength(BookStoreConsts.EntityLengths.More)]
        [Description("个人简介")]
        public string Profile { get; set; }

        /// <summary>
        /// 为序列化准备的私有构造函数
        /// 同时必须提供一个公有构造函数
        /// </summary>
        private Author()
        {

        }

        /// <summary>
        /// 这种包含全部参数的构造函数不是很适合面向数据库
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="birthDate"></param>
        /// <param name="profile"></param>
        public Author(Guid id, [NotNull] string name, DateTime birthDate, [CanBeNull] string profile = null) : base(id)
        {
            SetName(name);
            BirthDate = birthDate;
            Profile = profile;
        }

        public Author ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }

        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 64);
        }
    }
}
