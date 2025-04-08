using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Text;
using org.apache.zookeeper.data;

namespace AspNetCore.Zookeeper
{
    /// <summary>
    /// Zookeeper辅助类
    /// </summary>
    public class ZookeeperHelper : Watcher, IDisposable
    {
        /// <summary>
        /// Zookeeper路径分隔符
        /// </summary>
        string sep = "/";

        /// <summary>
        /// Zookeeper访问对象
        /// </summary>
        ZooKeeper zookeeper;

        /// <summary>
        /// Zookeeper集群地址
        /// </summary>
        string[] address;

        /// <summary>
        /// 路径监控节点列表
        /// </summary>
        ConcurrentDictionary<string, NodeWatcher> nodeWatchers = new ConcurrentDictionary<string, NodeWatcher>();

        /// <summary>
        /// 节点的默认权限
        /// </summary>
        List<ACL> defaultACL = ZooDefs.Ids.OPEN_ACL_UNSAFE;

        /// <summary>
        /// 默认的监听器
        /// </summary>
        DefaultWatcher defaultWatcher;

        /// <summary>
        /// 监控定时器
        /// </summary>
        System.Timers.Timer timer;

        /// <summary>
        /// 同步锁
        /// </summary>
        AutoResetEvent are = new AutoResetEvent(false);

        /// <summary>
        /// 是否正常关闭
        /// </summary>
        bool isClose = false;

        /// <summary>
        /// 回话超时时间
        /// </summary>
        public int SessionTimeout { get; set; } = 10000;

        /// <summary>
        /// 当前路径
        /// </summary>
        public string CurrentPath { get; private set; }

        /// <summary>
        /// 是否已连接Zookeeper
        /// </summary>
        public bool Connected { get { return zookeeper != null && (zookeeper.getState() == ZooKeeper.States.CONNECTED || zookeeper.getState() == ZooKeeper.States.CONNECTEDREADONLY); } }

        /// <summary>
        /// Zookeeper是否有写的权限
        /// </summary>
        public bool CanWrite { get { return zookeeper != null && zookeeper.getState() == ZooKeeper.States.CONNECTED; } }

        /// <summary>
        /// 数据编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.Default;

        /// <summary>
        /// 释放时发生
        /// </summary>
        public event Action OnDisposing;

        /// <summary>
        /// 在重新连接时发生
        /// </summary>
        public event Action OnConnected;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">集群地址(host:prot)</param>
        public ZookeeperHelper(params string[] address) : this(address, "")
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">集群地址(host:prot)</param>
        /// <param name="root">初始化根路经</param>
        public ZookeeperHelper(string[] address, string root)
        {
            this.address = address.ToArray();
            CurrentPath = string.IsNullOrWhiteSpace(root) ? sep : root;

            SetLogger(new ZookeeperLogger());

            timer = new System.Timers.Timer();
            timer.Interval = 5000;
            timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Zookeeper的日志设置
        /// </summary>
        /// <param name="log"></param>
        public static void SetLogger(ZookeeperLogger log)
        {
            ZooKeeper.LogLevel = log.LogLevel;
            ZooKeeper.LogToFile = log.LogToFile;
            ZooKeeper.LogToTrace = log.LogToTrace;
            ZooKeeper.CustomLogConsumer = log;
        }

        #region 私有方法

        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;

            if (Monitor.TryEnter(timer))//每次只能一个线程进去
            {
                if (!isClose)
                {
                    //Thread.Sleep(SessionTimeout);
                    if (!Connected)
                    {
                        try
                        {
                            zookeeper?.closeAsync();
                            are.Reset();
                            zookeeper = new ZooKeeper(string.Join(",", address), SessionTimeout, defaultWatcher);
                            if (are.WaitOne(SessionTimeout) && Connected)//会话未超时，表示成功连接
                            {
                                //挂载监听器
                                foreach (var key in nodeWatchers.Keys)
                                {
                                    NodeWatcher watcher;
                                    if (nodeWatchers.TryGetValue(key, out watcher))
                                    {
                                        WatchAsync(key, watcher, true).Wait();
                                    }
                                }
                                OnConnected?.Invoke();
                                Monitor.Exit(timer);
                                return;
                            }
                        }
                        catch { }
                        timer.Enabled = true;
                    }
                }

                Monitor.Exit(timer);
            }
        }

        /// <summary>
        /// 检查连接是否正常
        /// </summary>
        private void CheckConnection()
        {
            if (!Connected)
            {
                throw new Exception("fail to connect to the server:" + string.Join(",", address));
            }
        }

        /// <summary>
        /// 检查是否具有写的权限
        /// </summary>
        private void CheckWritable()
        {
            if (!CanWrite)
            {
                throw new Exception("this connection is in readonly mode");
            }
        }

        /// <summary>
        /// 连接数据成Zookeeper的路径格式
        /// </summary>
        /// <param name="paths">路径</param>
        /// <returns>连接后的路径</returns>
        private string Combine(params string[] paths)
        {
            List<string> list = new List<string>();
            foreach (var path in paths)
            {
                var ps = path.Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in ps)
                {
                    if (p == ".")//当前路径
                    {
                        continue;
                    }
                    else if (p == "..")//回退
                    {
                        if (list.Count == 0)
                        {
                            throw new ArgumentOutOfRangeException("path is out of range");
                        }

                        list.RemoveAt(list.Count - 1);
                    }
                    else
                    {
                        list.Add(p);
                    }
                }
            }

            return sep + string.Join(sep, list.ToArray());
        }

        /// <summary>
        /// 使用指定的分隔符连接路径
        /// </summary>
        /// <param name="sep">分隔符</param>
        /// <param name="paths">路径</param>
        /// <returns>连接后的路径</returns>
        private string MakePathName(string sep, params string[] paths)
        {
            List<string> list = new List<string>();
            foreach (var path in paths)
            {
                var ps = path.Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
                list.AddRange(ps);
            }

            return string.Join(sep, list.ToArray());
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isAbsolute">路径是否是绝对路径</param>
        /// <returns>绝对路径</returns>
        private string GetAbsolutePath(string path, bool isAbsolute)
        {
            if (!isAbsolute)
            {
                path = Combine(CurrentPath, path);
            }
            else
            {
                path = Combine(path);
            }
            return path;
        }

        #endregion

        /// <summary>
        /// 连接Zookeeper
        /// </summary>
        /// <returns>成功连接返回true,否则返回false</returns>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }
            if (zookeeper == null)
            {
                lock (this)
                {
                    defaultWatcher = defaultWatcher ?? new DefaultWatcher(this, are);
                    are.Reset();
                    zookeeper = new ZooKeeper(string.Join(",", address), SessionTimeout, defaultWatcher);
                    are.WaitOne(SessionTimeout);
                }
            }
            if (!Connected)
            {
                return false;
            }
            OnConnected?.Invoke();

            return true;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            isClose = true;
            if (Connected)
            {
                zookeeper.closeAsync().Wait();
            }

        }

        /// <summary>
        /// 监控回调
        /// </summary>
        /// <param name="event">回调事件</param>
        /// <returns>异步</returns>
        public override async Task process(WatchedEvent @event)
        {
            ZookeeperEvent ze = new ZookeeperEvent(@event);

            if (!string.IsNullOrEmpty(ze.Path))
            {
                NodeWatcher watcher;
                if (nodeWatchers.TryGetValue(ze.Path, out watcher))
                {
                    if (watcher != null)
                    {
                        try
                        {
                            watcher.Process(ze);
                        }
                        catch { }
                        await WatchAsync(ze.Path, watcher, true);//重新监控
                    }
                }
            }
        }

        /// <summary>
        /// 修改当前目录地址
        /// </summary>
        /// <param name="path"></param>
        public void ChangeDirectory(string path)
        {
            this.CurrentPath = Combine(path);
        }

        /// <summary>
        /// 切换到相对目录下
        /// </summary>
        /// <param name="path"></param>
        public void Goto(string path)
        {
            this.CurrentPath = Combine(this.CurrentPath, path);
        }

        /// <summary>
        /// 使用认证
        /// </summary>
        /// <param name="scheme">认证类型</param>
        /// <param name="auth">认证数据</param>
        public void Authorize(AuthScheme scheme, string auth = "")
        {
            CheckConnection();
            zookeeper.addAuthInfo(scheme.ToString().ToLower(), Encoding.GetBytes(auth));
        }

        #region 监听与取消

        /// <summary>
        /// 对当前路径添加监控
        /// </summary>
        /// <param name="delegate">监控</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAsync(WatcherEvent @delegate)
        {
            return await WatchAsync(CurrentPath, @delegate, true);
        }

        /// <summary>
        /// 对当前路径添加监控
        /// </summary>
        /// <param name="watcher">监控</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAsync(NodeWatcher watcher)
        {
            return await WatchAsync(CurrentPath, watcher, true);
        }

        /// <summary>
        /// 对指定路径添加监控
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="delegate">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAsync(string path, WatcherEvent @delegate, bool isAbsolutePath = false)
        {
            var array = await WatchManyAsync(new string[] { path }, @delegate, isAbsolutePath);
            return array.FirstOrDefault();
        }

        /// <summary>
        /// 对指定路径添加监控
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="watcher">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAsync(string path, NodeWatcher watcher, bool isAbsolutePath = false)
        {
            var array = await WatchManyAsync(new string[] { path }, watcher, isAbsolutePath);
            return array.FirstOrDefault();
        }

        /// <summary>
        /// 监控多个路径，但是不包括子路径
        /// </summary>
        /// <param name="paths">节点路径</param>
        /// <param name="delegate">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool[]> WatchManyAsync(string[] paths, WatcherEvent @delegate, bool isAbsolutePath = false)
        {
            var watcher = new NodeWatcher();
            watcher.AllTypeChanged += @delegate;
            return await WatchManyAsync(paths, watcher, isAbsolutePath);
        }

        /// <summary>
        /// 监控多个路径，但是不包括子路径
        /// </summary>
        /// <param name="paths">节点路径</param>
        /// <param name="watcher">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool[]> WatchManyAsync(string[] paths, NodeWatcher watcher, bool isAbsolutePath = false)
        {
            CheckConnection();
            List<bool> list = new List<bool>();
            foreach (var path in paths)
            {
                try
                {
                    var p = GetAbsolutePath(path, isAbsolutePath);
                    if (await zookeeper.existsAsync(p, this) != null)
                    {
                        nodeWatchers[p] = watcher;
                        list.Add(true);
                    }
                    else
                    {
                        nodeWatchers.TryRemove(p, out _);
                        list.Add(false);
                    }
                }
                catch
                {
                    list.Add(false);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 监控当前路径，包括子路径
        /// </summary>
        /// <param name="delegate">监控</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAllAsync(WatcherEvent @delegate)
        {
            return await WatchAllAsync(CurrentPath, @delegate, true);
        }

        /// <summary>
        /// 监控当前路径，包括子路径
        /// </summary>
        /// <param name="watcher">监控</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAllAsync(NodeWatcher watcher)
        {
            return await WatchAllAsync(CurrentPath, watcher, true);
        }

        /// <summary>
        /// 监控指定路径，包括子路径
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="delegate">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAllAsync(string path, WatcherEvent @delegate, bool isAbsolutePath = false)
        {
            var array = await WatchAllAsync(new string[] { path }, @delegate, isAbsolutePath);
            return array.FirstOrDefault();
        }

        /// <summary>
        /// 监控指定路径，包括子路径
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="watcher">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool> WatchAllAsync(string path, NodeWatcher watcher, bool isAbsolutePath = false)
        {
            var array = await WatchAllAsync(new string[] { path }, watcher, isAbsolutePath);
            return array.FirstOrDefault();
        }

        /// <summary>
        /// 监控所有路径，包括子路径
        /// </summary>
        /// <param name="paths">节点路径</param>
        /// <param name="delegate">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool[]> WatchAllAsync(string[] paths, WatcherEvent @delegate, bool isAbsolutePath = false)
        {
            var watcher = new NodeWatcher();
            watcher.AllTypeChanged += @delegate;
            return await WatchAllAsync(paths, watcher, isAbsolutePath);
        }

        /// <summary>
        /// 监控所有路径，包括子路径
        /// </summary>
        /// <param name="paths">节点路径</param>
        /// <param name="watcher">监控</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步，true表示成功，false表示失败</returns>
        public async Task<bool[]> WatchAllAsync(string[] paths, NodeWatcher watcher, bool isAbsolutePath = false)
        {
            CheckConnection();
            List<bool> list = new List<bool>();
            foreach (var path in paths)
            {
                try
                {
                    var p = GetAbsolutePath(path, isAbsolutePath);
                    if (await zookeeper.existsAsync(p, this) != null)
                    {
                        nodeWatchers[p] = watcher;
                        list.Add(true);

                        var result = await zookeeper.getChildrenAsync(p);
                        await WatchAllAsync(result.Children.Select(c => Combine(p, c)).ToArray(), watcher, true);
                    }
                    else
                    {
                        nodeWatchers.TryRemove(p, out _);
                        list.Add(false);
                    }
                }
                catch
                {
                    list.Add(false);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取消多个指定路径上的监控
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步</returns>
        public async Task CancelAsync(string path, bool isAbsolutePath = true)
        {
            await CancelAsync(new string[] { path }, isAbsolutePath);
        }

        /// <summary>
        /// 取消多个指定路径上的监控
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>异步</returns>
        public async Task CancelAsync(string[] paths, bool isAbsolutePath = true)
        {
            foreach (var path in paths)
            {
                var p = GetAbsolutePath(path, isAbsolutePath);
                nodeWatchers.TryRemove(p, out _);
                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// 获取指定路径上的监控
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <param name="isAbsolutePath">是否绝对路径</param>
        /// <returns>存在则返回监控对象，否则返回null</returns>
        public NodeWatcher GetWatcher(string path, bool isAbsolutePath = true)
        {
            path = GetAbsolutePath(path, isAbsolutePath);
            NodeWatcher watcher;
            if (nodeWatchers.TryGetValue(path, out watcher))
            {
                return watcher;
            }

            return null;
        }

        #endregion

        #region 基本数据操作

        /// <summary>
        /// 当前节点是否存在
        /// </summary>
        /// <returns>存在返回true，否则返回false</returns>
        public bool Exists()
        {
            return ExistsAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// 指定节点是否存在（相对当前节点）
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <returns>存在返回true，否则返回false</returns>
        public bool Exists(string path)
        {
            return ExistsAsync(path).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 指定节点是否存在
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>存在返回true，否则返回false</returns>
        public bool ExistsByAbsolutePath(string absolutePath)
        {
            return ExistsByAbsolutePathAsync(absolutePath).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 当前节点是否存在
        /// </summary>
        /// <returns>异步，存在返回true，否则返回false</returns>
        public async Task<bool> ExistsAsync()
        {
            return await ExistsByAbsolutePathAsync(CurrentPath);
        }

        /// <summary>
        /// 指定节点是否存在（相对当前节点）
        /// </summary>
        /// <param name="path">节点路径</param>
        /// <returns>异步，存在返回true，否则返回false</returns>
        public async Task<bool> ExistsAsync(string path)
        {
            path = Combine(CurrentPath, path);
            return await ExistsByAbsolutePathAsync(path);
        }

        /// <summary>
        /// 指定节点是否存在
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>异步，存在返回true，否则返回false</returns>
        public async Task<bool> ExistsByAbsolutePathAsync(string absolutePath)
        {
            absolutePath = Combine(absolutePath);
            return await zookeeper.existsAsync(absolutePath, false) != null;
        }

        /// <summary>
        /// 添加或者修改当前路径上的数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="persistent">是否持久节点</param>
        /// <param name="sequential">是否顺序节点</param>
        /// <returns>znode节点名，不包含父节点路径</returns>
        public string SetData(string value, bool persistent = false, bool sequential = false)
        {
            return SetDataAsync(value, persistent, sequential).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 添加或者修改指定相对路径上的数据
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <param name="value">数据</param>
        /// <param name="persistent">是否持久节点</param>
        /// <param name="sequential">是否顺序节点</param>
        /// <returns>znode节点名，不包含父节点路径</returns>
        public string SetData(string path, string value, bool persistent = false, bool sequential = false)
        {
            return SetDataAsync(path, value, persistent, sequential).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 添加或者修改指定绝对路径上的数据
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="value">数据</param>
        /// <param name="persistent">是否持久节点</param>
        /// <param name="sequential">是否顺序节点</param>
        /// <returns>znode节点名，不包含父节点路径</returns>
        public string SetDataByAbsolutePath(string absolutePath, string value, bool persistent = false, bool sequential = false)
        {
            return SetDataByAbsolutePathAsync(absolutePath, value, persistent, sequential).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 添加或者修改当前路径上的数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="persistent">是否持久节点</param>
        /// <param name="sequential">是否顺序节点</param>
        /// <returns>znode节点名，不包含父节点路径</returns>
        public async Task<string> SetDataAsync(string value, bool persistent = false, bool sequential = false)
        {
            return await SetDataByAbsolutePathAsync(CurrentPath, value, persistent, sequential);
        }

        /// <summary>
        /// 添加或者修改指定相对路径上的数据
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <param name="value">数据</param>
        /// <param name="persistent">是否持久节点</param>
        /// <param name="sequential">是否顺序节点</param>
        /// <returns>znode节点名，不包含父节点路径</returns>
        public async Task<string> SetDataAsync(string path, string value, bool persistent = false, bool sequential = false)
        {
            path = Combine(CurrentPath, path);
            return await SetDataByAbsolutePathAsync(path, value, persistent, sequential);
        }

        /// <summary>
        /// 添加或者修改指定绝对路径上的数据
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="value">数据</param>
        /// <param name="persistent">是否持久节点</param>
        /// <param name="sequential">是否顺序节点</param>
        /// <returns>znode节点名，不包含父节点路径</returns>
        public async Task<string> SetDataByAbsolutePathAsync(string absolutePath, string value, bool persistent = false, bool sequential = false)
        {
            CheckConnection();
            CheckWritable();

            absolutePath = Combine(absolutePath);
            //父节点处理
            var splits = absolutePath.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < splits.Length - 1; i++)
            {
                var path = Combine(splits.Take(i + 1).ToArray());
                if (await zookeeper.existsAsync(path, false) == null)
                {
                    await zookeeper.createAsync(path, new byte[0], defaultACL, persistent ?
                       sequential ? CreateMode.PERSISTENT_SEQUENTIAL : CreateMode.PERSISTENT :
                       sequential ? CreateMode.EPHEMERAL_SEQUENTIAL : CreateMode.EPHEMERAL);
                }
            }

            if (await zookeeper.existsAsync(absolutePath, false) == null)
            {
                absolutePath = await zookeeper.createAsync(absolutePath, Encoding.GetBytes(value), defaultACL, persistent ?
                    sequential ? CreateMode.PERSISTENT_SEQUENTIAL : CreateMode.PERSISTENT :
                    sequential ? CreateMode.EPHEMERAL_SEQUENTIAL : CreateMode.EPHEMERAL);
            }
            else
            {
                await zookeeper.setDataAsync(absolutePath, Encoding.GetBytes(value));
            }

            return absolutePath.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        }

        /// <summary>
        /// 获取指定相对路径上的数据
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>相对路径上的数据</returns>
        public string GetData(string path)
        {
            return GetDataAsync(path).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取指定绝对路径上的数据
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>相对路径上的数据</returns>
        public string GetDataByAbsolutePath(string absolutePath)
        {
            return GetDataByAbsolutePathAsync(absolutePath).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取指定相对路径上的数据
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>相对路径上的数据</returns>
        public async Task<string> GetDataAsync(string path)
        {
            path = Combine(CurrentPath, path);
            return await GetDataByAbsolutePathAsync(path);
        }

        /// <summary>
        /// 获取指定绝对路径上的数据
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>绝对路径上的数据</returns>
        public async Task<string> GetDataByAbsolutePathAsync(string absolutePath)
        {
            CheckConnection();
            absolutePath = Combine(absolutePath);
            if (await zookeeper.existsAsync(absolutePath, false) == null)
            {
                return "";
            }
            var data = await zookeeper.getDataAsync(absolutePath, false);
            return Encoding.GetString(data.Data);
        }

        /// <summary>
        /// 获取指定节点及其字节点的所有值，使用路径做键返回字典型
        /// </summary>
        /// <param name="sep"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, string>> GetDictionaryAsync(string sep = ":")
        {
            CheckConnection();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            async Task action(string path)
            {
                try
                {
                    var result = await zookeeper.getChildrenAsync(path, false);
                    string name = MakePathName(sep, path);
                    dict[name] = await GetDataByAbsolutePathAsync(path);
                    foreach (var child in result.Children)
                    {
                        var p = Combine(path, child);
                        await action(p);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            await action(CurrentPath);
            return dict;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <param name="order">是否按时间排序</param>
        /// <returns>子节点数组（节点路径不含父节点路径）</returns>
        public async Task<string[]> GetChildrenAsync(string path, bool order = false)
        {
            path = Combine(CurrentPath, path);
            return await GetChildrenByAbsolutePathAsync(path, order);
        }

        /// <summary>
        /// 获取指定路径绝对路径下的子节点
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="order">是否按时间排序</param>
        /// <returns>子节点数组（节点路径不含父节点路径）</returns>
        public async Task<string[]> GetChildrenByAbsolutePathAsync(string absolutePath, bool order = false)
        {
            var result = await zookeeper.getChildrenAsync(absolutePath, false);
            if (!order)
            {
                return result.Children.ToArray();
            }

            List<(string, long)> list = new List<(string, long)>();
            foreach (var child in result.Children)
            {
                var p = Combine(absolutePath, child);
                var stat = await zookeeper.existsAsync(p, false);
                if (stat != null)
                {
                    list.Add((child, stat.getCtime()));
                }
            }

            return list.OrderBy(l => l.Item2).Select(l => l.Item1).ToArray();
        }

        /// <summary>
        /// 移除当前路径节点
        /// </summary>
        public void Delete()
        {
            DeleteAsync().Wait();
        }

        /// <summary>
        /// 移除相对当前的指定路径节点及子节点
        /// </summary>
        /// <param name="path">相对路径</param>
        public void Delete(string path)
        {
            DeleteAsync(path).Wait();
        }

        /// <summary>
        /// 移除指定绝对路径节点及子节点
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        public void DeleteByAbsolutePath(string absolutePath)
        {
            DeleteByAbsolutePathAsync(absolutePath).Wait();
        }

        /// <summary>
        /// 移除当前路径节点
        /// </summary>
        public async Task DeleteAsync()
        {
            await DeleteByAbsolutePathAsync(CurrentPath);
        }

        /// <summary>
        /// 移除相对当前的指定路径节点及子节点
        /// </summary>
        /// <param name="path">相对路径</param>
        public async Task DeleteAsync(string path)
        {
            path = Combine(CurrentPath, path);
            await DeleteByAbsolutePathAsync(path);
        }

        /// <summary>
        /// 移除指定绝对路径节点及子节点
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        public async Task DeleteByAbsolutePathAsync(string absolutePath)
        {
            if (await ExistsByAbsolutePathAsync(absolutePath))
            {
                var children = await GetChildrenByAbsolutePathAsync(absolutePath);
                foreach (var child in children)
                {
                    var path = Combine(absolutePath, child);
                    await DeleteByAbsolutePathAsync(path);
                }

                absolutePath = Combine(absolutePath);
                await zookeeper.deleteAsync(absolutePath);
            }
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            OnDisposing?.Invoke();
            Close();
            timer?.Dispose();
            nodeWatchers?.Clear();
            are?.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// 默认的监听器，用于初始化使用
        /// </summary>
        private class DefaultWatcher : Watcher
        {
            /// <summary>
            /// waithandle同步
            /// </summary>
            EventWaitHandle ewh;

            /// <summary>
            /// 辅助类
            /// </summary>
            ZookeeperHelper zookeeperHelper;

            public DefaultWatcher(ZookeeperHelper zookeeperHelper, EventWaitHandle ewh)
            {
                this.ewh = ewh;
                this.zookeeperHelper = zookeeperHelper;
            }

            /// <summary>
            /// 回调
            /// </summary>
            /// <param name="event">监听事件对象</param>
            /// <returns></returns>
            public override Task process(WatchedEvent @event)
            {
                var state = @event.getState();
                if (state == Event.KeeperState.ConnectedReadOnly || state == Event.KeeperState.SyncConnected)//连接时
                {
                    ewh.Set();
                }
                else if ((state == Event.KeeperState.Expired) && !zookeeperHelper.isClose)//回话过期重新建立连接
                {
                    zookeeperHelper.timer.Enabled = true;
                }

                return Task.FromResult(1);
            }
        }
    }
}
