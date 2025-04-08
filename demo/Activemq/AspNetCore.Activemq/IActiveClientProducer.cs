using AspNetCore.Activemq.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Activemq
{
    public interface IActiveClientProducer : IDisposable
    {
        /// <summary>
        /// 发送消息到队列
        /// </summary>
        /// <param name="activeMessages"></param>
        /// <returns></returns>
        Task SendAsync(params ActiveMessage[] activeMessages);
        /// <summary>
        /// 发送消息到Topic
        /// </summary>
        /// <param name="activeMessages"></param>
        /// <returns></returns>
        Task PublishAsync(params ActiveMessage[] activeMessages);
    }
}
