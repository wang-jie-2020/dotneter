using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Demo.Models
{
    /// <summary>
    /// 作者
    /// </summary>
    public class Author 
    {
        //public Guid Id { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string Profile { get; set; }
    }
}
