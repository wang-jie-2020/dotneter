using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Activemq
{
    public interface IActiveProducerFactory
    {
        /// <summary>
        /// 创建一个生产者
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IActiveClientProducer Create(string name);
    }
}
