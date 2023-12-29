using org.apache.zookeeper;

namespace AspNetCore.Zookeeper
{
    /// <summary>
    /// Zookeeper事件数据
    /// </summary>
    public class ZookeeperEvent
    {
        public ZookeeperEvent(WatchedEvent @event)
        {
            switch (@event.getState())
            {
                case Watcher.Event.KeeperState.AuthFailed: State = EventState.AuthFailed; break;
                case Watcher.Event.KeeperState.ConnectedReadOnly: State = EventState.ConnectedReadOnly; break;
                case Watcher.Event.KeeperState.Disconnected: State = EventState.Disconnected; break;
                case Watcher.Event.KeeperState.Expired: State = EventState.Expired; break;
                case Watcher.Event.KeeperState.SyncConnected: State = EventState.SyncConnected; break;
            }

            switch (@event.get_Type())
            {
                case Watcher.Event.EventType.NodeChildrenChanged: Type = EventType.NodeChildrenChanged; break;
                case Watcher.Event.EventType.NodeCreated: Type = EventType.NodeCreated; break;
                case Watcher.Event.EventType.NodeDataChanged: Type = EventType.NodeDataChanged; break;
                case Watcher.Event.EventType.NodeDeleted: Type = EventType.NodeDeleted; break;
                case Watcher.Event.EventType.None: Type = EventType.None; break;
            }

            Path = @event.getPath();
        }

        /// <summary>
        /// 当前连接状态
        /// </summary>
        public EventState State { get; private set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType Type { get; private set; }

        /// <summary>
        /// 事件路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public enum EventState
        {
            /// <summary>
            /// 超时
            /// </summary>
            Expired = -112,
            /// <summary>
            /// 连接已断开
            /// </summary>
            Disconnected = 0,
            /// <summary>
            /// 已建立连接
            /// </summary>
            SyncConnected = 3,
            /// <summary>
            /// 认证失败
            /// </summary>
            AuthFailed = 4,
            /// <summary>
            /// 已建立连接，但是只支持只读模式
            /// </summary>
            ConnectedReadOnly = 5
        }

        /// <summary>
        /// 时间类型
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// 空类型，如：建立连接时
            /// </summary>
            None = -1,
            /// <summary>
            /// 节点创建时
            /// </summary>
            NodeCreated = 1,
            /// <summary>
            /// 节点删除时
            /// </summary>
            NodeDeleted = 2,
            /// <summary>
            /// 节点数据改变时
            /// </summary>
            NodeDataChanged = 3,
            /// <summary>
            /// 节点增加子节点时
            /// </summary>
            NodeChildrenChanged = 4
        }
    }

    /// <summary>
    /// 监控事件委托
    /// </summary>
    /// <param name="event"></param>
    public delegate void WatcherEvent(ZookeeperEvent @event);
}