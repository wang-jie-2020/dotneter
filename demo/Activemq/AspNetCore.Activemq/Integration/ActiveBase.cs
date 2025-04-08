using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AspNetCore.Activemq.Integration
{
    public abstract class ActiveBase : IDisposable
    {
#if DEBUG
        public static ActiveTrace Trace { get; set; } = new ActiveTrace(TraceLevel.Warn, (level, message) => Console.WriteLine($"{level}:{message}"));
#else
        public static ActiveTrace Trace { get; set; } = null;
#endif

        static ActiveBase()
        {
            Tracer.Trace = Trace;
        }

        IConnectionFactory connectionFactory;

        bool isDisposed = false;
        string brokerUri;

        protected ActiveBase(bool isCluster, string[] hostAndPorts, string query, ActiveProtocol protocol)
        {
            if (hostAndPorts == null || hostAndPorts.Length == 0)
            {
                throw new ArgumentException("invalid hostAndPorts！", nameof(hostAndPorts));
            }
            if (!isCluster && hostAndPorts?.Length != 1)
            {
                throw new ArgumentException("None Cluster mode only support one host", nameof(hostAndPorts));
            }
            var p = GetProtocol(protocol);
            if (isCluster)
            {
                brokerUri = MakeClusterBrokerUri(p, hostAndPorts, query);
            }
            else
            {
                brokerUri = MakeBrokerUri(p, hostAndPorts.First(), query);
            }
            connectionFactory = CreateConnectionFactory(p, brokerUri);
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        private IConnectionFactory CreateConnectionFactory(string protocol, string brokerUri)
        {
            var factory = NMSConnectionFactory.CreateConnectionFactory(new Uri($"{protocol}://active"));
            factory.BrokerUri= Apache.NMS.Util.URISupport.CreateCompatibleUri(brokerUri); 
            //IConnectionFactory factory = new Apache.NMS.ActiveMQ.ConnectionFactory(brokerUri);// new NMSConnectionFactory(brokerUri);
            //IConnectionFactory factory = new Apache.NMS.AMQP.ConnectionFactory(brokerUri);// new NMSConnectionFactory(brokerUri);
            return factory;
        }
        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <returns></returns>
        protected IConnection CreateConnection()
        {
            CheckDisposed();
            IConnection connection = connectionFactory.CreateConnection(UserName, Password);
            return connection;
        }
        protected void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
        public virtual void Dispose()
        {
            isDisposed = true;
        }
        public override string ToString()
        {
            return $"{this.GetType().FullName}:{brokerUri}";
        }

        protected bool IsConnected(IConnection connection)
        {
            if (connection is Apache.NMS.AMQP.NmsConnection nmsConnection)
            {
                return nmsConnection.IsConnected;
            }
            if (connection is Apache.NMS.ActiveMQ.Connection activeMQConnection)
            {
                return activeMQConnection.ITransport?.IsConnected ?? false;
            }
            return false;
        }
        protected void InitListenConnection(IConnection connection, ListenOptions listenOptions)
        {
            if (listenOptions.PrefetchCount != null)
            {
                if (connection is Apache.NMS.ActiveMQ.Connection activeMQConnection)
                {
                    if (listenOptions.FromQueue)
                    {
                        activeMQConnection.PrefetchPolicy.QueuePrefetch = listenOptions.PrefetchCount.Value;
                    }
                    else
                    {
                        activeMQConnection.PrefetchPolicy.TopicPrefetch = listenOptions.PrefetchCount.Value;
                    }
                }
                else if (connection is Apache.NMS.AMQP.NmsConnection nmsConnection)
                {

                }
            }
            if (!string.IsNullOrEmpty(listenOptions.ClientId))
            {
                connection.ClientId = listenOptions.ClientId;
            }
        }
        private static string GetProtocol(ActiveProtocol protocol)
        {
            switch (protocol)
            {
                case ActiveProtocol.Amqp: return "amqp";
                default: return "tcp";
            }
        }
        private static string MakeClusterBrokerUri(string protocol, string[] hostAndPorts, string query)
        {
            List<string> list = new List<string>();
            foreach (var hostAndPort in hostAndPorts)
            {
                var brokerUri = MakeOneBrokerUri(protocol, hostAndPort);
                list.Add(brokerUri);
            }
            string uri = string.Join(",", list);
            query = query?.Trim()?.TrimStart('?');
            return string.IsNullOrEmpty(query) ? $"failover:({uri})" : $"failover:({uri})?{query}";
        }
        private static string MakeBrokerUri(string protocol, string hostAndPort, string query)
        {
            var uri = MakeOneBrokerUri(protocol, hostAndPort);
            query = query?.Trim()?.TrimStart('?');
            return string.IsNullOrEmpty(query) ? $"{uri}" : $"{uri}?{query}";
        }
        private static string MakeOneBrokerUri(string protocol, string hostAndPort)
        {
            var hasProtocol = hostAndPort.Contains("://");//包含协议
            if (hasProtocol)
            {
                return hostAndPort;
            }

            var splits = hostAndPort.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var port = 61616;
            if (splits.Count == 1)//只有一段，则认为是host
            {
                switch (protocol)
                {
                    case "amqp": port = 5672; break;
                    default: (protocol, port) = ("tcp", 61616); break;
                }
                splits.Add(port.ToString());
            }
            else if (splits.Count == 2) //两端，则认为是host+port
            {
                return $"{protocol}://{splits[0]}:{splits[1]}";
            }
            throw new FormatException("invalid fotmat of 'hostAndPort',consider the format 'host:port'");
        }

    }
}
