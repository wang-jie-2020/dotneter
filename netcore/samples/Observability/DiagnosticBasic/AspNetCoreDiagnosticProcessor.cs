using System.Diagnostics;
using System.Reactive;
using System.Reactive.Subjects;

namespace Observability.DiagnosticBasic;

public class AspNetCoreDiagnosticProcessor : BackgroundService
{
    private readonly ILogger<AspNetCoreDiagnosticProcessor> _logger;

    public AspNetCoreDiagnosticProcessor(ILogger<AspNetCoreDiagnosticProcessor> logger)
    {
        _logger = logger;
    }
    public string ListenerName { get; } = "Microsoft.AspNetCore";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        DiagnosticListener.AllListeners.Subscribe(new AnonymousObserver<DiagnosticListener>((listener) =>
        {
            if (listener.Name == ListenerName)
            {
                listener.Subscribe(new AnonymousObserver<KeyValuePair<string, object?>>((track) =>
                {
                    _logger.LogInformation(track.Key + " - " + track.Value);
                }));
            }
        }));

        await Task.CompletedTask;
    }
}