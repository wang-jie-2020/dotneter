using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Consul.Configurations
{
    public class ConsulConfigurationProvider : ConfigurationProvider, IDisposable
    {
        ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();
        ConsulConfigurationOptions consulConfigurationOptions;
        IDisposable _changeTokenRegistration;
        ConsulClient consulClient;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ulong lastIndex = 0;
        Task task;

        public ConsulConfigurationSource ConsulConfigurationSource { get; private set; }

        public ConsulConfigurationProvider(ConsulConfigurationSource consulConfigurationSource)
        {
            this.ConsulConfigurationSource = consulConfigurationSource;
            this.consulConfigurationOptions = consulConfigurationSource.ConsulConfigurationOptions;

            consulClient = new ConsulClient(options =>
            {
                options.Address = new Uri(consulConfigurationOptions.Address);//consul集群地址
                if (!string.IsNullOrEmpty(consulConfigurationOptions.Datacenter))
                {
                    options.Datacenter = consulConfigurationOptions.Datacenter;
                }
                if (!string.IsNullOrEmpty(consulConfigurationOptions.Token))
                {
                    options.Token = consulConfigurationOptions.Token;
                }
            });

            if (consulConfigurationOptions.Mode == WatchMode.Watch)
            {
                _changeTokenRegistration = ChangeToken.OnChange(
                    () => Watch(),
                    () =>
                    {
                        Load();
                    });
            }
            else if (consulConfigurationOptions.Mode == WatchMode.Poll)
            {
                task = new Task(() =>
                {
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        Load(true, cancellationTokenSource.Token);
                        Thread.Sleep(100);//暂停一下
                    }
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
        /// <param name="wait"></param>
        /// <param name="cancellationToken"></param>
        public void Load(bool wait, CancellationToken cancellationToken = default)
        {
            try
            {
                var data = Process(wait, cancellationToken);
                if (data == null || data.Count == 0) return;
                Data = data;
            }
            catch
            {
                return;
            }

            OnReload();

            if (task?.Status == TaskStatus.Created)
            {
                task.Start();
            }
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public override void Load()
        {
            Load(false);
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reload()
        {
            var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            _changeTokenRegistration?.Dispose();
            cancellationTokenSource.Cancel();
            consulClient.Dispose();
        }
        /// <summary>
        /// 获取Consul中的数据
        /// </summary>
        /// <param name="wait"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private IDictionary<string, string> Process(bool wait, CancellationToken cancellationToken = default)
        {
            var queryOptions = new QueryOptions
            {
                WaitTime = wait ? consulConfigurationOptions.Interval : TimeSpan.FromSeconds(1),
                WaitIndex = wait ? lastIndex : 0
            };
            var result = consulClient.KV.List(consulConfigurationOptions.Prefix ?? "", queryOptions, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
            if (result.StatusCode == HttpStatusCode.OK && result.Response != null)
            {
                if (!wait)
                {
                    return ToDictionary(result.Response);
                }

                if (result.LastIndex > lastIndex)
                {
                    lastIndex = result.LastIndex < lastIndex ? 0 : result.LastIndex;
                    return ToDictionary(result.Response);
                }
            }

            return null;
        }
        private IDictionary<string, string> ToDictionary(params KVPair[] kVPairs)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var length = (consulConfigurationOptions.Prefix ?? "").Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Length;
            foreach (var kvPair in kVPairs)
            {
                var split = kvPair.Key.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (consulConfigurationOptions.RemovePrefix)
                {
                    split = split.Skip(length).ToArray();
                }
                string key = ConfigurationPath.Combine(split);
                dict[key] = kvPair.Value == null ? null : consulConfigurationOptions.Encoding.GetString(kvPair.Value);
            }
            return dict;
        }

    }
}
