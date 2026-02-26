using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAT
{
    public class CallerHosted : BackgroundService
    {
        private readonly FailureCaller failureCaller;
        private readonly EnergyCaller energyCaller;

        public CallerHosted(FailureCaller failureCaller, EnergyCaller energyCaller)
        {
            this.failureCaller = failureCaller;
            this.energyCaller = energyCaller;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Task.Run(() =>
            //{
            //    failureCaller.ExecuteAsync(stoppingToken);
            //});

            Task.Run(() =>
            {
                energyCaller.ExecuteAsync(stoppingToken);
            });

            return Task.CompletedTask;
        }
    }
}
