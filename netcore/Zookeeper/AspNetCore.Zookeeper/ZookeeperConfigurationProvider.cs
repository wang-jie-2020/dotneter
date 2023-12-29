using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Zookeeper
{
    public class ZookeeperConfigurationProvider : ConfigurationProvider, IDisposable
    {
        IDisposable _changeTokenRegistration;
        ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        /// <summary>
        /// 监控对象
        /// </summary>
        ZookeeperConfigurationWatcher zookeeperConfigurationWatcher;

        public ZookeeperConfigurationSource ZookeeperConfigurationSource { get; private set; }

        public ZookeeperConfigurationProvider(ZookeeperConfigurationSource zookeeperConfigurationSource)
        {
            this.ZookeeperConfigurationSource = zookeeperConfigurationSource;
            this.zookeeperConfigurationWatcher = new ZookeeperConfigurationWatcher(this);

            if (zookeeperConfigurationSource.ZookeeperOptions.ReloadOnChange)
            {
                _changeTokenRegistration = ChangeToken.OnChange(
                    () => Watch(),
                    () =>
                    {
                        Thread.Sleep(zookeeperConfigurationSource.ZookeeperOptions.ReloadDelay);
                        Load();
                    });
            }
        }

        /// <summary>
        /// 获取ReloadToken
        /// </summary>
        /// <returns></returns>
        private IChangeToken Watch()
        {
            return _reloadToken;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public override void Load()
        {
            Data = zookeeperConfigurationWatcher.Process();

            OnReload();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reload()
        {
            var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }

        //public override bool TryGet(string key, out string value)
        //{
        //    return Data.TryGetValue(key, out value);
        //}

        //public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        //{
        //    var prefix = parentPath == null ? string.Empty : parentPath + ConfigurationPath.KeyDelimiter;

        //    var list = Data
        //        .Where(kv => kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        //        .Select(kv => Segment(kv.Key, prefix.Length))
        //        .Concat(earlierKeys)
        //        .OrderBy(k => k, ConfigurationKeyComparer.Instance);

        //    return list;
        //}

        //private static string Segment(string key, int prefixLength)
        //{
        //    var indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
        //    return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
        //}

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            _changeTokenRegistration?.Dispose();
        }
    }
}
