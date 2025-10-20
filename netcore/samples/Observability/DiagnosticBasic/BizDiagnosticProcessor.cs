using System.Diagnostics;
using System.Reactive;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Observability.DiagnosticBasic;

public class BizDiagnosticProcessor : IHostedService
{
    private readonly ILogger<BizDiagnosticProcessor> _logger;

    public BizDiagnosticProcessor(ILogger<BizDiagnosticProcessor> logger)
    {
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        DiagnosticListener.AllListeners.Subscribe(new BizDiagnosticProcessorObserver());
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    class BizDiagnosticProcessorObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == BizDiagnosticConsts.Listener)
            {
                //value.Subscribe(new BizDiagnosticObserver());
                //value.Subscribe(new BizDiagnosticObserver(), (name, _, _) => name == startEventName);
                
                //虽然提供了重载, 但似乎不太必要, 因为Adapter已经根据注解存在与否判断是否Enabled
                value.SubscribeWithAdapter(new BizDiagnosticAdapter());
                //value.SubscribeWithAdapter(new BizDiagnosticAdapter(), (name, _, _) => name == endEventName);
            }
        }
    }

    class BizDiagnosticObserver : IObserver<KeyValuePair<string, object?>>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(KeyValuePair<string, object?> value)
        {
            Console.WriteLine(value.Key + ": " + value.Value);
        }
    }

    class BizDiagnosticAdapter
    {
        [DiagnosticName(BizDiagnosticConsts.StartEvent)]
        public void StartEvent(DateTime Times)
        {
            
        }

        // 参数名称、参数类型来自于发布时的类型,如果出现不一致就不能正常获得参数,是的,转object也不行...
        [DiagnosticName(BizDiagnosticConsts.EndEvent)]
        public void EndEvent(object cannot_get_parameters)
        {

        }
    }
}