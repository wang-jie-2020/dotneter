using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Rabbitmq.Integration
{
    public class MessageOptions
    {
        /// <summary>
        /// User Id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public byte Priority { get; set; }
        /// <summary>
        /// 是否持久化消息
        /// </summary>
        public bool Persistent { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 消息响应标识
        /// </summary>
        public string CorrelationId { get; set; }
        /// <summary>
        /// 消息头
        /// </summary>
        public IDictionary<string, object> Headers { get; set; }
        /// <summary>
        /// 消息有效时间(或时间搓)
        /// </summary>
        public string Expiration { get; set; }
        /// <summary>
        /// Content-Type
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Content-Encoding
        /// </summary>
        public string ContentEncoding { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 消息类型名称
        /// </summary>
        public string Type { get; set; }
    }
}
