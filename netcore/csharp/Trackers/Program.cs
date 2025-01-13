using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Trackers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            builder.Services.AddRouting(o =>
            {
                o.LowercaseUrls = true;
                o.LowercaseQueryStrings = true;
            });

            //web主机中默认的DiagnosticListener
            //---var listener = new DiagnosticListener("Microsoft.AspNetCore");
            // DiagnosticListener.AllListeners.Subscribe((listener) =>
            // {
            //     if (listener.Name == "Microsoft.AspNetCore")
            //     {
            //         listener.Subscribe(new LocalObserver()!);
            //         listener.Subscribe(
            //            onNext: (pair) => { },
            //            onError: ex => { },
            //            onCompleted: () => { });
            //     };
            // });

            var app = builder.Build();

            // var diagnosticListener = app.Services.GetRequiredService<DiagnosticListener>();
            // diagnosticListener.SubscribeWithAdapter(new LocalAdapterObserver());

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.Map("/ping", app => { app.Run(async context => { await context.Response.WriteAsync("pong"); }); });

            app.MapControllers();

            app.Run();
        }
    }

    // public class LocalObserver : IObserver<KeyValuePair<string, object>>
    // {
    //     public void OnCompleted()
    //     {
    //
    //     }
    //
    //     public void OnError(Exception error)
    //     {
    //         Console.WriteLine($"{nameof(LocalObserver)}----------------------");
    //         Console.WriteLine($"{error.Message}");
    //     }
    //
    //     public void OnNext(KeyValuePair<string, object> pair)
    //     {
    //         Console.WriteLine($"{nameof(LocalObserver)}----------------------");
    //         Console.WriteLine($"{pair.Key}-{pair.Value}");
    //     }
    // }

    // public class LocalAdapterObserver
    // {
    //     private ConcurrentDictionary<string, long> startTimes = new ConcurrentDictionary<string, long>();
    //
    //     [DiagnosticName("Microsoft.AspNetCore.Hosting.BeginRequest")]
    //     public void BeginRequest(HttpContext httpContext, long timestamp)
    //     {
    //         Console.WriteLine($"{nameof(LocalAdapterObserver)}----------------------");
    //         Console.WriteLine($"Request {httpContext.TraceIdentifier} Begin:{httpContext.Request.GetDisplayUrl()}");
    //         startTimes.TryAdd(httpContext.TraceIdentifier, timestamp);//记录请求开始时间
    //     }
    //
    //     [DiagnosticName("Microsoft.AspNetCore.Hosting.EndRequest")]
    //     public void EndRequest(HttpContext httpContext, long timestamp)
    //     {
    //         Console.WriteLine($"{nameof(LocalAdapterObserver)}----------------------");
    //         startTimes.TryGetValue(httpContext.TraceIdentifier, out long startTime);
    //         var elapsedMs = (timestamp - startTime) / TimeSpan.TicksPerMillisecond;//计算耗时
    //         Console.WriteLine(
    //             $"Request {httpContext.TraceIdentifier} End: Status Code is {httpContext.Response.StatusCode},Elapsed {elapsedMs}ms");
    //         startTimes.TryRemove(httpContext.TraceIdentifier, out _);
    //     }
    // }
}