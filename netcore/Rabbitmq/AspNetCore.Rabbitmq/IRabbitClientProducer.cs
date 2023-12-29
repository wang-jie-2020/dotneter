using AspNetCore.Rabbitmq.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Rabbitmq
{
    public interface IRabbitClientProducer : IDisposable
    {
        /// <summary>
        /// 普通的往队列发送消息
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="options"></param>
        void Publish(string[] messages, MessageOptions options = null);
        /// <summary>
        /// 使用交换机发送消息
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="messages"></param>
        /// <param name="options"></param>
        void Publish(string routingKey, string[] messages, MessageOptions messageOptions = null);
    }
}
