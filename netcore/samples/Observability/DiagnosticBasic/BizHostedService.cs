using System.Diagnostics;

namespace Observability.DiagnosticBasic;

public class BizHostedService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // var diagnostic = new DiagnosticListener(listenerName);
        //
        // Task.Run(async () =>
        // {
        //     while (true)
        //     {
        //         if (diagnostic.IsEnabled(startEventName))
        //         {
        //             diagnostic.Write(startEventName, new
        //             {
        //                 Times = DateTime.Now
        //             });
        //         }
        //         
        //         if (diagnostic.IsEnabled(endEventName))
        //         {
        //             diagnostic.Write(endEventName, DateTime.Now);
        //         }
        //
        //         await Task.Delay(1000, cancellationToken);
        //     }
        // });
        

        
        
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}