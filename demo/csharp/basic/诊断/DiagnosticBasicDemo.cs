using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.AspNetCore.Mvc.Abstractions;


namespace basic.诊断
{
    internal class DiagnosticBasicDemo
    {
        private static DiagnosticSource diagnosticSource;

        private const string listenerName = "demo.basic.diagnostic.listener";
        private const string eventName = "demo.basic.diagnostic.event";

        private object locker = new();

        static DiagnosticBasicDemo()
        {
            diagnosticSource = new DiagnosticListener(listenerName);
        }

        /*
         * MSDN上描述diagnosticSource.IsEnabled主要是一种性能设计,防止输出太多不必要的诊断日志
         * diagnosticSource.write()的参数即KeyValuePair<string, object>>,由订阅者IObservable进行处理
         */
        void Method999()
        {
            if (diagnosticSource.IsEnabled(eventName))
            {
                diagnosticSource.Write(eventName, new { Attachment = "whatever" });
            }
        }

        static IDisposable? listenerSubscription = null;

        /// <summary>
        ///  单个diagnosticSource的注册
        /// </summary>
        void Method1()
        {
            listenerSubscription?.Dispose();

            listenerSubscription = (diagnosticSource as DiagnosticListener).Subscribe((data) =>
            {
                Console.WriteLine(data.Key);
                Console.WriteLine(data.Value?.ToString());
            }, (ex) => { }, () => { });
        }

        static IDisposable? listenersSubscription = null;

        /// <summary>
        ///  通过listenerName在AllListeners中注册
        ///     注意AllListeners.Subscribe()与listener.Subscribe的订阅参数是不同的
        /// </summary>
        void Method2()
        {
            DiagnosticListener.AllListeners.Subscribe((listener) =>
            {
                if (listener.Name == listenerName)
                {
                    lock (locker)
                    {
                        listenersSubscription?.Dispose();
                    }

                    listenersSubscription = listener.Subscribe((data) =>
                    {
                        Console.WriteLine(data.Key);
                        Console.WriteLine(data.Value?.ToString());
                    }, (ex) => { }, () => { });
                }
            });
        }

        /// <summary>
        ///     基于Microsoft.Extensions.DiagnosticAdapter
        /// </summary>
        void Method3()
        {
            (diagnosticSource as DiagnosticListener).SubscribeWithAdapter(new DemoDiagnosticAdapter());
        }

        /// <summary>
        ///     Microsoft.Extensions.DiagnosticAdapter
        /// </summary>
        void Method4()
        {
            var adapter = new DemoDiagnosticAdapter();

            DiagnosticListener.AllListeners.Subscribe((listener) =>
            {
                if (listener.Name == adapter.ListenerName)
                {
                    listener.SubscribeWithAdapter(adapter, (key) =>
                    {
                        //键值对的key
                        return true;
                    });
                }
            });
        }

        class DemoDiagnosticAdapter
        {
            public string ListenerName { get; } = listenerName;

            [DiagnosticName(eventName)]
            public void DemoEvent(string attachment)
            {
                System.Console.WriteLine($"监听{attachment}");
            }
        }
    }
}
