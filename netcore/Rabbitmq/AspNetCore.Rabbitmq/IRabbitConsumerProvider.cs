﻿using AspNetCore.Rabbitmq.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Rabbitmq.Consumers
{
    public interface IRabbitConsumerProvider: IDisposable
    {
        RabbitConsumer Consumer { get; }

        /// <summary>
        /// 开始监听消费消息
        /// </summary>
        /// <returns></returns>
        Task ListenAsync();
    }
}
