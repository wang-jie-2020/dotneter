using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using SuperSocket.ProtoBase;

namespace SuperSocketServer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = SuperSocketHostBuilder
                .Create<StringPackageInfo, CommandLinePipelineFilter>()
                //.UsePackageHandler(async (session, package) =>
                //{
                //    var result = 0;

                //    switch (package.Key.ToUpper())
                //    {
                //        case ("ADD"):
                //            result = package.Parameters
                //                .Select(p => int.Parse(p))
                //                .Sum();
                //            break;

                //        case ("SUB"):
                //            result = package.Parameters
                //                .Select(p => int.Parse(p))
                //                .Aggregate((x, y) => x - y);
                //            break;

                //        case ("MULT"):
                //            result = package.Parameters
                //                .Select(p => int.Parse(p))
                //                .Aggregate((x, y) => x * y);
                //            break;
                //    }

                //    await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
                //})
                .ConfigureLogging((hostCtx, loggingBuilder) =>
                {
                    loggingBuilder.AddConsole();
                })
                .ConfigureSuperSocket(options =>
                {
                    //此处可以直接配置块替换
                    options.Name = "Echo Server";
                    options.Listeners = new[] {
                        new ListenOptions
                        {
                            Ip = "Any",
                            Port = 4040
                        }
                    };
                });

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}