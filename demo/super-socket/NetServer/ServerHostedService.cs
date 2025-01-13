using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NetServer.Server;

namespace NetServer
{
    internal class ServerHostedService : BackgroundService, IHostedService
    {
        private readonly SocketServerInitializer _socketServerInitializer;

        public ServerHostedService(SocketServerInitializer socketServerInitializer)
        {
            _socketServerInitializer = socketServerInitializer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _socketServerInitializer.Initialize();
            await Task.CompletedTask;
        }
    }
}
