namespace AspNetCore.Zookeeper
{
    /// <summary>
    /// 监控对象
    /// </summary>
    public class NodeWatcher
    {
        /// <summary>
        /// 节点创建时调用事件
        /// </summary>
        public event WatcherEvent NodeCreated;

        /// <summary>
        /// 节点删除时调用事件
        /// </summary>
        public event WatcherEvent NodeDeleted;

        /// <summary>
        /// 节点数据改变时调用事件
        /// </summary>
        public event WatcherEvent NodeDataChanged;

        /// <summary>
        /// 节点增加子节点时调用事件
        /// </summary>
        public event WatcherEvent NodeChildrenChanged;

        /// <summary>
        /// 不区分类型，所有的类型都会调用
        /// </summary>
        public event WatcherEvent AllTypeChanged;

        /// <summary>
        /// 触发，执行事件
        /// </summary>
        /// <param name="event"></param>
        public void Process(ZookeeperEvent @event)
        {
            try
            {
                switch (@event.Type)
                {
                    case ZookeeperEvent.EventType.NodeChildrenChanged:
                        NodeChildrenChanged?.Invoke(@event);
                        break;
                    case ZookeeperEvent.EventType.NodeCreated:
                        NodeCreated?.Invoke(@event);
                        break;
                    case ZookeeperEvent.EventType.NodeDeleted:
                        NodeDeleted?.Invoke(@event);
                        break;
                    case ZookeeperEvent.EventType.NodeDataChanged:
                        NodeDataChanged?.Invoke(@event);
                        break;
                }

                AllTypeChanged?.Invoke(@event);
            }
            catch { }
        }
    }
}