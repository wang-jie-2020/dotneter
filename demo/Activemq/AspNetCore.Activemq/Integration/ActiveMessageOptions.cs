using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Activemq.Integration
{
    public class ActiveMessageOptions
    {
        /// <summary>
        /// 是否使用事务
        /// </summary>
        public bool Transactional { get; set; }
    }
}
