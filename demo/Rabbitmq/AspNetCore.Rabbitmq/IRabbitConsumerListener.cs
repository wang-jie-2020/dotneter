using AspNetCore.Rabbitmq.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Rabbitmq
{
    public interface IRabbitConsumerListener
    {
        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="recieveResult"></param>
        /// <returns></returns>
        Task ConsumeAsync(RecieveResult recieveResult);
    }
}
